using Product.API.Data;

namespace Product.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;
        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public async Task<Entities.Product> CreateProduct(Entities.Product request)
        {
            await _context.AddAsync(request);
            _context.SaveChanges();
            return request;
        }
    }
}