using RCL.Dtos.Request;
using RCL.Dtos.Response;

namespace RCL.Dtos;

public class ProductCategoryDto
{
    public ProductRequestDto ProductRequest { get; set; } = new();
    public List<CategoryDto> Categories { get; set; } = [];
}