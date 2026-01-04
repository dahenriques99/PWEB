namespace RCL.Dtos.Response;

public class OrderDetailsDto : OrderSummaryDto
{
    public sealed class OrderLineDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public byte[]? ImageData { get; set; }
    }

    public List<OrderLineDto> Items { get; set; } = new();
    public String DeliveryMode { get; set; }
}

public class OrderSummaryDto
{
    public int OrderId { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = default!;
}