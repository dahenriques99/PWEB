using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Infrastructure.Entities.enums;
using MyMedia.Infrastructure.Entities;
using RCL.Dtos.Request;
using WebAPI.Repositories;
using WebAPI.utils;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IProductRepository productRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ProductQuery query)
    {
        var products = await productRepository.GetProducts(query);
        var productDtos = products.Select(Mapper.FromProduct);
        return Ok(productDtos);
    }
        
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await productRepository.GetProduct(id);
        if (product is null) return NotFound();
        var dto = Mapper.FromProduct(product);
        return Ok(dto);
    }

    [Authorize(Roles = nameof(UserRoles.Supplier))]
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] CreateProductDto createProductDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var result = await productRepository.AddProduct(userId, createProductDto);

        if (result.Errors.Any())
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Product);
    }
}