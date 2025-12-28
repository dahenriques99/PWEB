using System.ComponentModel.DataAnnotations;
using MyMedia.Infrastructure;

namespace MyMedia.Domain.Entities;

public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime Date { get; set; } = DateTime.Now;
    
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}