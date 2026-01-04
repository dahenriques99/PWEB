using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyMedia.Infrastructure.Entities;

public class DeliveryMode
{
    public int Id { get; set; }

    [StringLength(100)]
    [Required]
    public string? Name { get; set; }

    [StringLength(200)]
    public string? Details { get; set; }

    [JsonIgnore]
    public ICollection<Order>? Orders { get; set; }
}