namespace RCL.Dtos;

public class ProductCategoryDto
{
    public ProductDto Product { get; set; } = new();
    public List<CategoryDto> Categories { get; set; } = [];
}