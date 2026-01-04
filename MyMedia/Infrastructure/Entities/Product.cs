using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using MyMedia.Infrastructure.Entities.enums;

namespace MyMedia.Infrastructure.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string SupplierId { get; set; }
    
    public ApplicationUser Supplier { get; set; }

    [Required(ErrorMessage = "The Product Name is required", AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "The Product Name must not exceed 50 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "The Product Description must not exceed 200 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "The Product Price is required")]
    [Range(0.01, int.MaxValue, ErrorMessage = "The Product Price cannot be negative")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "The Product Final Price is required")]
    [Range(0.01, int.MaxValue, ErrorMessage = "The Product Final Price cannot be negative")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal FinalPrice { get; set; }

    [Required(ErrorMessage = "The Product Stock is required")]
    [Range(0, int.MaxValue, ErrorMessage = "The Product Price cannot be negative")]
    public int Stock { get; set; }
    public byte[]? ImageData { get; set; }
    
    [NotMapped]
    public IFormFile? Image { get; set; }
    
    public ProductStatus Status { get; set; }
    
    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}