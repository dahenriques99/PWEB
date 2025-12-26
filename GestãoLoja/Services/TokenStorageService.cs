namespace GestãoLoja.Services;

public class TokenStorageService
{ 
    private string? _token;
    private DateTime? _expirationTime;
    
    public void SetToken(string token, int expiresIn)
    {
        _token = token;
        _expirationTime = DateTime.UtcNow.AddSeconds(expiresIn);
    }

    public string? GetToken()
    {
        if(_token != null && _expirationTime.HasValue && DateTime.UtcNow < _expirationTime)
        {
            return _token;
        }
        ClearToken();
        return null;
    }

    private void ClearToken()
    {
        _token = null;
        _expirationTime = null;
    }
}