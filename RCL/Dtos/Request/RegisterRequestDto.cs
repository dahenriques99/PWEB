using System.ComponentModel.DataAnnotations;

namespace RCL.Dtos.Request;

public class RegisterRequestDto
{
    [Required]
    [StringLength(50)]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ\s'-]+$", ErrorMessage = "Name can only contain letters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ\s'-]+$", ErrorMessage = "Surname can only contain letters.")]
    public string Surname { get; set; } = string.Empty;

    [Required, EmailAddress] public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$",
        ErrorMessage = "Password must contain at least one uppercase letter, one digit, and one special character.")]
    public string Password { get; set; } = string.Empty;

    [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF must contain exactly 9 digits.")]
    public string? Nif { get; set; }
}