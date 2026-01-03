using System.ComponentModel.DataAnnotations;
namespace MyMedia.Infrastructure.Entities;
public class Category
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "The Category Name is required", AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "The Category Name must not exceed 50 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "The Category Description must not exceed 200 characters")]
    public string? Description { get; set; }

    public int? ParentCategoryId { get; set; }
    
    public ICollection<ProductCategory> ProductCategories { get; set; }
        = new List<ProductCategory>();
}