using System.ComponentModel.DataAnnotations;

namespace MyMedia.Domain.Entities;

public class ProductCategory
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; }

    [Required] 
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}