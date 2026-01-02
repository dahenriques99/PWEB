using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using MyMedia.Domain.Entities;
using MyMedia.Infrastructure.Entities;
using MyMedia.Infrastructure.Entities.enums;

namespace MyMedia.Infrastructure;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    // [Required(ErrorMessage = "The user name is required.", AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "The user name must not exceed 50 characters.")]
    public string? Name { get; set; }
    
    [StringLength(50, ErrorMessage = "The user surname must not exceed 50 characters.")]
    public string? Surname { get; set; }
    
    [StringLength(9, ErrorMessage = "The NIF must be 9 digits.")]
    public string? Nif { get; set; }
    
    public UserStatus Status { get; set; } = UserStatus.Pending;
    
    public UserType ClientType { get; set; } = UserType.Client;
    
    public byte[]? ImageData { get; set; } = [];
        
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Product> SuppliedProducts { get; set; } = new List<Product>();
}