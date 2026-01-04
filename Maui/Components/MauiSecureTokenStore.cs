using RCL.Components.Services;

public sealed class MauiSecureTokenStore : ITokenStore
{
    private const string Key = "auth_token";

    public async Task<string?> GetAsync()
    {
        try { return await SecureStorage.GetAsync(Key); }
        catch { return null; } // emulator/device may block secure storage in some cases
    }

    public async Task SetAsync(string token)
    {
        try { await SecureStorage.SetAsync(Key, token); }
        catch { Preferences.Set(Key, token); } // fallback if SecureStorage fails
    }

    public Task ClearAsync()
    {
        SecureStorage.Remove(Key);
        Preferences.Remove(Key);
        return Task.CompletedTask;
    }
}