using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyMedia.Infrastructure.Entities;

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
    
    [JsonIgnore]
    [DefaultValue("Store Pickup")]
    public int? DeliveryId { get; set; }

    public DeliveryMode DeliveryMode { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}