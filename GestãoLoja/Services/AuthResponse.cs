namespace GestãoLoja.Services;

public class AuthResponse(string accessToken, string tokenType, int expiresIn, string email)
{
    public string AccessToken { get; set; } = accessToken;
    public string TokenType { get; set; } = tokenType;
    public int ExpiresIn { get; set; } = expiresIn;
    public string Email { get; set; } = email;
}