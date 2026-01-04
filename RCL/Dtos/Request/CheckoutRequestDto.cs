namespace RCL.Dtos.Request;

public sealed class CheckoutRequestDto
{
    public List<CheckoutItemDto> Items { get; set; } = new();
    public int DeliveryMode { get; set; }
}

public sealed class CheckoutItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}