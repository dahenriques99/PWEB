namespace GestãoLoja.Services;

public class LoginResult
{

    public bool Success { get; set; }
    public string Message { get; set; }
    public string? Email { get; set; }
}