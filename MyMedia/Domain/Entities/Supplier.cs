using System.ComponentModel.DataAnnotations;

namespace MyMedia.Domain.Entities;

public class Supplier
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "The Supplier Name is required", AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "The Supplier Name must not exceed 50 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "The Supplier description must not exceed 200 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "The Supplier email is required", AllowEmptyStrings = false)]
    [EmailAddress(ErrorMessage = "The Supplier email must be a valid email address")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "The Supplier Phone Number must ve a valid phone number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Supplier NIF is required", AllowEmptyStrings = false)]
    [StringLength(9, MinimumLength = 9, ErrorMessage = "The Supplier NIF must have 9 characters")]
    public string Nif { get; set; } = string.Empty;
    public byte[]? LogoData { get; set; } = [];

    public ICollection<Product> Products { get; set; } = new List<Product>();
}