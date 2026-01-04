using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RCL.Components.Services;

public sealed class AuthStateService(ITokenStore store)
{
    public event Action? Changed;

    private const string TokenKey = "auth_token";
    public string? Token { get; private set; }
    public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Token);

    public IReadOnlyCollection<string> Roles { get; private set; } = Array.Empty<string>();
    public bool IsSupplier => Roles.Contains("Supplier");
    public bool IsClient => Roles.Contains("Client");


    public async Task InitializeAsync()
    {
        Token = await store.GetAsync();

        Roles = ParseRoles(Token);
        Changed?.Invoke();
    }

    public async Task SetTokenAsync(string token)
    {
        Token = token;
        Roles = ParseRoles(token);
        
        await store.SetAsync(token);
        Changed?.Invoke();
    }

    public async Task ClearAsync()
    {
        Token = null;
        Roles = Array.Empty<string>();
        
        await store.ClearAsync();
        Changed?.Invoke();
    }

    private static IReadOnlyCollection<string> ParseRoles(string? jwt)
    {
        if (string.IsNullOrWhiteSpace(jwt)) return Array.Empty<string>();

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var roles = token.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                .Select(c => c.Value)
                .Distinct()
                .ToArray();

            return roles;
        }
        catch
        {
            return Array.Empty<string>();
        }
    }
}