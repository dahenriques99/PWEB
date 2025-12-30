using RCL.Dtos;

namespace RCL.Services;

public class AuthService
{
    private string? _accessToken;
    private string? _refreshToken;
    public UserDto User = new();

    // Store tokens in-memory
    public void StoreTokens(string accessToken, string refreshToken)
    {
        _accessToken = accessToken;
        _refreshToken = refreshToken;
    }

    public async void StoreUser(UserDto userDto)
    {
        User = userDto;
    }

    // Get tokens from in-memory storage
    public (string? accessToken, string? refreshToken) GetTokens()
    {
        return (_accessToken, _refreshToken);
    }

    // Remove tokens (clear in-memory data)
    public void RemoveTokens()
    {
        _accessToken = null;
        _refreshToken = null;
    }

    // Check if the user is authenticated (i.e., access token exists)
    public bool IsAuthenticated()
    {
        return !string.IsNullOrEmpty(_accessToken);
    }
}