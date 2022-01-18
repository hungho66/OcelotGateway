using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using OcelotGateway.DelegateHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
                });

builder.Services.AddOcelot()
                .AddCacheManager(settings => settings.WithDictionaryHandle())
                .AddDelegatingHandler<MyDelegatingHandler>();


var app = builder.Build();

app.UseRouting();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
