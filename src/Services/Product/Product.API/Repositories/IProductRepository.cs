namespace Product.API.Repositories
{
    public interface IProductRepository
    {
        Task<Product.API.Entities.Product> CreateProduct(Product.API.Entities.Product request);
    }
}