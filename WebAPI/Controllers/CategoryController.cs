using Microsoft.AspNetCore.Mvc;
using MyMedia.Infrastructure.Entities;
using WebAPI.Repositories;
using WebAPI.utils;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(ICategoryRepository categoryRepository) : ControllerBase
{
    // GET: api/Category
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await categoryRepository.GetCategories();
        var categoriesDtos = categories.Select(Mapper.FromCategory);
        return Ok(categoriesDtos);
    }

    // GET: api/Category/5
    [HttpGet("{id}")]
    public async Task<Category> GetCategory(int id)
    {
        return await categoryRepository.GetCategory(id);
    }
}