namespace RCL.Components.Services;

public sealed class AuthStateService
{
    public string? AccessToken { get; private set; }

    public bool IsLoggedIn => !string.IsNullOrWhiteSpace(AccessToken);

    public event Action? Changed;

    public void SetToken(string token)
    {
        AccessToken = token;
        Changed?.Invoke();
    }

    public void Clear()
    {
        AccessToken = null;
        Changed?.Invoke();
    }
}