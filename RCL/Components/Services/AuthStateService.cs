using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RCL.Components.Services;

public sealed class AuthStateService(IKeyValueStore store)
{
    public event Action? Changed;
    private const string TokenKey = "auth_token";
    public string? Token { get; private set; }
    public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Token);
    public string? UserId { get; private set; }
    public IReadOnlyCollection<string> Roles { get; private set; } = Array.Empty<string>();
    public bool IsSupplier => Roles.Contains("Supplier");


    public async Task InitializeAsync()
    {
        Token = await store.GetAsync(TokenKey);

        ParseToken(Token);
        Changed?.Invoke();
    }

    public async Task SetTokenAsync(string token)
    {
        Token = token;
        ParseToken(Token);
        
        await store.SetAsync(TokenKey, token);
        Changed?.Invoke();
    }

    public async Task ClearAsync()
    {
        Token = null;
        Roles = Array.Empty<string>();
        
        await store.RemoveAsync(TokenKey);
        Changed?.Invoke();
    }

    private void ParseToken(string? jwt)
    {
        UserId = null;
        Roles = Array.Empty<string>();

        if (string.IsNullOrWhiteSpace(jwt))
            return;

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            
            UserId = token.Claims
                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)
                ?.Value;

            Roles = token.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                .Select(c => c.Value)
                .Distinct()
                .ToArray();
        }
        catch
        {
            UserId = null;
            Roles = Array.Empty<string>();
        }
    }
}