namespace RCL.Components.Services;

public interface IKeyValueStore
{
    Task<string?> GetAsync(string key);
    Task SetAsync(string key, string value);
    Task RemoveAsync(string key);
}