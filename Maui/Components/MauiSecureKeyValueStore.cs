using RCL.Components.Services;

namespace Maui.Components;

public sealed class MauiSecureKeyValueStore : IKeyValueStore
{
    public Task<string?> GetAsync(string key)
        => Task.FromResult(Preferences.Get(key, (string?)null));

    public Task SetAsync(string key, string value)
    {
        Preferences.Set(key, value);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        Preferences.Remove(key);
        return Task.CompletedTask;
    }
}