using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using RCL.Components.Services;

public sealed class ProtectedSessionTokenStore(ProtectedSessionStorage storage) : ITokenStore
{
    private const string Key = "auth_token";

    public async Task<string?> GetAsync()
    {
        var result = await storage.GetAsync<string>(Key);
        return result.Success ? result.Value : null;
    }

    public Task SetAsync(string token) => storage.SetAsync(Key, token).AsTask();
    public Task ClearAsync() => storage.DeleteAsync(Key).AsTask();
}