using System.ComponentModel.DataAnnotations;

namespace MyMedia.Domain.Entities;

public class ProductSnapshot
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; }

    [Required]
    public int OrderId { get; set; }
    public Order Order { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}