using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using MyMedia.Domain.Entities;

namespace MyMedia.Infrastructure;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    // [Required(ErrorMessage = "The user name is required.", AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "The user name must not exceed 50 characters.")]
    public string? Name { get; set; }
    [StringLength(50, ErrorMessage = "The user surname must not exceed 50 characters.")]
    public string? Surname { get; set; }
    // [Length(9, 9, ErrorMessage = "The NIF must be 9 digits.")]
    // public long? Nif { get; set; }
    public bool IsActive { get; set; } = true;
    //public byte[] ImageData { get; set; } = [];
    // public UserProfile UserProfile { get; set; }

    public ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}