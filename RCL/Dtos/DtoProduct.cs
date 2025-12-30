using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCL.Dtos;

public class DtoProduct
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    [StringLength(200)]
    [Required]
    public string Name { get; set; } = string.Empty;
    [StringLength(200)]
    public string? Description { get; set; }
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public byte[] ImageData { get; set; } = [];
    // public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    // public ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
    // public ICollection<OrderItem> ProductSnapshots { get; set; } = new List<OrderItem>();
}