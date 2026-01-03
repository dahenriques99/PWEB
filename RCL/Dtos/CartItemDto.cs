namespace RCL.Dtos;

public sealed class CartItemDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public byte[]? ImageData { get; set; }
}