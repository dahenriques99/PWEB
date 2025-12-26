using System.ComponentModel.DataAnnotations;
using MyMedia.Infrastructure;

namespace MyMedia.Domain.Entities;

public class CartProduct
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int Quantity { get; set; }
}