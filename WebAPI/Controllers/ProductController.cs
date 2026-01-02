using Microsoft.AspNetCore.Mvc;
using MyMedia.Domain.Entities;
using RCL.Dtos.Request;
using RCL.Dtos.Response;
using WebAPI.Repositories;
using WebAPI.utils;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductRepository productRepository) : ControllerBase
    {
        // GET: api/Product?category
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQuery query)
        {
            var products = await productRepository.GetProducts(query);
            var productDtos = products.Select(Mapper.FromProduct);
            return Ok(productDtos);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<Product> GetProduct(int id)
        {
            return await productRepository.GetProduct(id);
        }

        // PUT: api/Product
        [HttpPost]
        public async Task<int> AddProduct([FromBody] ProductRequestDto productRequest)
            => await productRepository.AddProduct(productRequest);
    }
}