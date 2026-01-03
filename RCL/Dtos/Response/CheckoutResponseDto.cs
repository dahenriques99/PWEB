namespace RCL.Dtos.Response;

public sealed class CheckoutResponseDto
{
    public int OrderId { get; set; }
    public decimal Total { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
}