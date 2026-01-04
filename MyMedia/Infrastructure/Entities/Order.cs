using System.ComponentModel.DataAnnotations;
using MyMedia.Infrastructure.Entities.enums;

namespace MyMedia.Infrastructure.Entities;

public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }

    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;
    
    public int DeliveryModeId { get; set; }

    public DeliveryMode DeliveryMode { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}