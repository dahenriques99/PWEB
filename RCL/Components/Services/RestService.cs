using System.Net.Http.Headers;
using System.Net.Http.Json;
using RCL.Dtos.Request;
using RCL.Dtos.Response;

namespace RCL.Components.Services;

public class RestService
{
    private readonly HttpClient _httpClient;
    private readonly AuthStateService _authStateService;

    public RestService(HttpClient httpClient, AuthStateService auth)
    {
        _httpClient = httpClient;
        _authStateService = auth;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategories()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDto>>("/api/Category");
            return response ?? Enumerable.Empty<CategoryDto>();
            ;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error fetching categories: {ex.Message}");
            return Enumerable.Empty<CategoryDto>();
        }
    }

    public async Task<IEnumerable<ProductDto>> GetProducts(ProductQuery query)
    {
        try
        {
            var parts = new List<string>();

            if (query.CategoryIds is { Count: > 0 })
                parts.AddRange(query.CategoryIds.Distinct().Select(id => $"CategoryIds={id}"));

            if (query.MatchAllCategories)
                parts.Add("MatchAllCategories=true");

            var url = "/api/Product" + (parts.Count > 0 ? "?" + string.Join("&", parts) : "");

            var response = await _httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>(url);
            return response ?? Enumerable.Empty<ProductDto>();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error fetching products: {ex.Message}");
            return Enumerable.Empty<ProductDto>();
        }
    }

    public async Task<HttpResponseMessage> Register(RegisterRequestDto request)
    {
        return await _httpClient.PostAsJsonAsync("/api/auth/register", request);
    }
    
    public async Task<HttpResponseMessage> Login(LoginRequestDto request)
    {
        return await _httpClient.PostAsJsonAsync("/api/auth/login", request);
    }
    
    private string? _token;

    
    public Task SetToken(string token)
    {
        _authStateService.SetToken(token);
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        return Task.CompletedTask;
    }
    
    public Task Logout()
    {
        _authStateService.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = null;
        return Task.CompletedTask;
    }
}