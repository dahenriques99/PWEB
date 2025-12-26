using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MyMedia.Domain.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    [Required(ErrorMessage = "The Product Name is required", AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "The Product Name must not exceed 50 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "The Product Description must not exceed 200 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "The Product Price is required")]
    [Range(0.01, int.MaxValue, ErrorMessage = "The Product Price cannot be negative")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "The Product Stock is required")]
    [Range(0, int.MaxValue, ErrorMessage = "The Product Price cannot be negative")]
    public int Stock { get; set; }

    public byte[]? ImageData { get; set; }

    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
    
    public ICollection<ProductSnapshot> ProductSnapshots { get; set; } = new List<ProductSnapshot>();
    
    [NotMapped]
    public IFormFile? Image { get; set; }
}