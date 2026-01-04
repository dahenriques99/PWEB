using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using RCL.Components.Services;

namespace BlazorWeb.Components;

public sealed class ProtectedSessionTokenStore(ProtectedSessionStorage storage) : IKeyValueStore
{
    public async Task<string?> GetAsync(string key)
    {
        var r = await storage.GetAsync<string>(key);
        return r.Success ? r.Value : null;
    }

    public Task SetAsync(string key, string value)
        => storage.SetAsync(key, value).AsTask();

    public Task RemoveAsync(string key)
        => storage.DeleteAsync(key).AsTask();
}