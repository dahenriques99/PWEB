using System.ComponentModel.DataAnnotations;

namespace MyMedia.Infrastructure.Entities;

public class OrderItem
{

    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; }

    [Required]
    public int OrderId { get; set; }
    public Order Order { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}