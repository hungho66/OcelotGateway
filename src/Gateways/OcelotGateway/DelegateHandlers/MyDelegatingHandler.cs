using System.Net;
using Newtonsoft.Json;
using Ocelot.Middleware;
using OcelotGateway.Dto;
using OcelotGateway.Model;

namespace OcelotGateway.DelegateHandlers
{
    public class MyDelegatingHandler : DelegatingHandler
    {
        private IHttpContextAccessor _httpContext;
        public MyDelegatingHandler(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            var downstreamRoute = _httpContext.HttpContext.Items?.DownstreamRouteHolder()?.Route?.DownstreamRoute?.FirstOrDefault();
            if (downstreamRoute != null)
            {
                if (downstreamRoute.Key == "BarKey")
                {
                    //TODO: Mapping Product DTO (Client) -> Product Model (API)
                    var bodyString = await request.Content.ReadAsStringAsync();

                    // Deserialize into a MyClass that will hold your request body data
                    var productRequestDto = JsonConvert.DeserializeObject<ProductRequestDto>(bodyString);

                    //Convert To Request API (ProductModel == productRequestDto) - can use AutoMapper
                    var productRequestModel = new ProductRequestModel();
                    productRequestModel.Id = productRequestDto.Id;
                    productRequestModel.Name = productRequestDto.Ten;
                    productRequestModel.Category = productRequestDto.Category;
                    productRequestModel.Summary = productRequestDto.Summary;
                    productRequestModel.Description = productRequestDto.Description;
                    productRequestModel.ImageFile = productRequestDto.ImageFile;
                    productRequestModel.Price = productRequestDto.Price;

                    //Convert ProductModel json -> request API
                    var productRequestMapping = JsonConvert.SerializeObject(productRequestModel);
                    request.Content = new StringContent(productRequestMapping, System.Text.Encoding.UTF8, "application/json");

                    var response = await base.SendAsync(request, cancellationToken);

                    //TODO: Mapping response
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var productResponseModel = JsonConvert.DeserializeObject<ProductResponseModel>(responseJson);
                    var productResponseDto = new ProductRequestDto();


                    productResponseDto.Id = productResponseModel.Id;
                    productResponseDto.Ten = productResponseModel.Name;
                    productResponseDto.Category = productResponseModel.Category;
                    productResponseDto.Summary = productResponseModel.Summary;
                    productResponseDto.Description = productResponseModel.Description;
                    productResponseDto.ImageFile = productResponseModel.ImageFile;
                    productResponseDto.Price = productResponseModel.Price;

                    var resultJson = JsonConvert.SerializeObject(productResponseDto);
                    var result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(resultJson, System.Text.Encoding.UTF8, "application/json")
                    };

                    tsc.SetResult(result);   // Also sets the task state to "RanToCompletion"
                };

                // Note: TaskCompletionSource creates a task that does not contain a delegate.
                return tsc.Task.Result;

            }
            else if (downstreamRoute.Key == "Test")
            {
                return await base.SendAsync(request, cancellationToken);
            }
            return await base.SendAsync(request, cancellationToken);
        }

    }
}