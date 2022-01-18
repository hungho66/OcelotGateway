using System.Net;
using Product.API.Repositories;

namespace Product.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<Entities.Product> CreateProductAsync([FromBody] Entities.Product command)
        {
            var result = await _productRepository.CreateProduct(command);
            return result;
        }
    }
}