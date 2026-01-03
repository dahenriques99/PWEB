namespace RCL.Dtos.Response;

public sealed class ProductResponseDto
{
    public int Id { get; set; }
    public string SupplierName { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Status { get; set; }
    public byte[]? ImageData { get; set; } = [];
}