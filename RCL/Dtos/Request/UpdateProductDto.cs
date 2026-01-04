namespace RCL.Dtos.Request;

public sealed class UpdateProductDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public List<int>? CategoryIds { get; set; } = new();
    public byte[]? ImageData { get; set; } 
}