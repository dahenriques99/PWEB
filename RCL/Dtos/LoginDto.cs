using System.ComponentModel.DataAnnotations;

namespace RCL.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "The Email is required", AllowEmptyStrings = false)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    
    [Required(ErrorMessage = "The Password is required", AllowEmptyStrings = false)]
    public string Password {get; set;} = string.Empty;
}