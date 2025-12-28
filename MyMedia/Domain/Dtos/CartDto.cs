namespace MyMedia.Domain.Dtos;

public class CartDto
{
    public string UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}