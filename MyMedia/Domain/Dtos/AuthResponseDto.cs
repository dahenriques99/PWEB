namespace MyMedia.Domain.Dtos;

public class AuthResponseDto
{
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
    public string Email { get; set; }
}