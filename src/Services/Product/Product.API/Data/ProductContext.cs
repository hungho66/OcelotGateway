using Microsoft.EntityFrameworkCore;

namespace Product.API.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }
        public DbSet<Product.API.Entities.Product> Products { get; set; }
    }
}