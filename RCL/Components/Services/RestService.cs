using System.Net.Http.Headers;
using System.Net.Http.Json;
using RCL.Dtos.Request;
using RCL.Dtos.Response;

namespace RCL.Components.Services;

public class RestService(HttpClient httpClient, AuthStateService auth)
{
    public async Task<IEnumerable<CategoryDto>> GetAllCategories()
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<IEnumerable<CategoryDto>>("/api/Category");
            return response ?? Enumerable.Empty<CategoryDto>();
            ;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error fetching categories: {ex.Message}");
            return Enumerable.Empty<CategoryDto>();
        }
    }

    public async Task<IEnumerable<ProductResponseDto>> GetProducts(ProductQuery query)
    {
        try
        {
            var parts = new List<string>();

            if (query.CategoryIds is { Count: > 0 })
                parts.AddRange(query.CategoryIds.Distinct().Select(id => $"CategoryIds={id}"));

            if (query.MatchAllCategories)
                parts.Add("MatchAllCategories=true");

            if (query.OnlyInStock)
                parts.Add("OnlyInStock=true");
            
            var url = "/api/Product" + (parts.Count > 0 ? "?" + string.Join("&", parts) : "");

            var response = await httpClient.GetFromJsonAsync<IEnumerable<ProductResponseDto>>(url);
            return response ?? Enumerable.Empty<ProductResponseDto>();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error fetching products: {ex.Message}");
            return Enumerable.Empty<ProductResponseDto>();
        }
    }

    public Task<OrderDetailsDto?> GetOrder(int id)
    {
        EnsureAuthHeader();
        return httpClient.GetFromJsonAsync<OrderDetailsDto>($"/api/order/{id}");
    }
    
    public async Task<ProductResponseDto?> GetProduct(int id)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<ProductResponseDto>($"/api/product/{id}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error fetching product {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<List<DeliveryMethodsResponseDto>> GetDeliveryMethods()
    {
        var response = await httpClient.GetFromJsonAsync<List<DeliveryMethodsResponseDto>>("/api/DeliveryMethods");
        
        return response ?? Enumerable.Empty<DeliveryMethodsResponseDto>().ToList();
    }
    
    public async Task<HttpResponseMessage> Checkout(CheckoutRequestDto request)
    {
        EnsureAuthHeader();
        return await httpClient.PostAsJsonAsync("/api/order/checkout", request);
    }

    public async Task<ProfileDto?> GetProfile()
    {
        EnsureAuthHeader();
        return await httpClient.GetFromJsonAsync<ProfileDto>("/api/Profile");
    }

    public async Task<IEnumerable<OrderSummaryDto>> GetUserOrders()
    {
        EnsureAuthHeader();
        return await httpClient.GetFromJsonAsync<IEnumerable<OrderSummaryDto>>("/api/Profile/orders")
               ?? Enumerable.Empty<OrderSummaryDto>();
    }
    
    public async Task<IEnumerable<ProductResponseDto>> GetUserProducts()
    {
        EnsureAuthHeader();
        return await httpClient.GetFromJsonAsync<IEnumerable<ProductResponseDto>>("/api/Profile/products")
               ?? Enumerable.Empty<ProductResponseDto>();
    }
    
    public Task<HttpResponseMessage> AddProduct(CreateProductDto dto)
    {
        EnsureAuthHeader();
        return httpClient.PostAsJsonAsync("/api/product", dto);
    }
    
    public Task<HttpResponseMessage> UpdateProduct(int id, UpdateProductDto dto)
    {
        EnsureAuthHeader();
        return httpClient.PutAsJsonAsync($"/api/product/{id}", dto);
    }
    
    public async Task<HttpResponseMessage> Register(RegisterRequestDto request)
    {
        return await httpClient.PostAsJsonAsync("/api/auth/register", request);
    }
    
    public async Task<HttpResponseMessage> Login(LoginRequestDto request)
    {
        return await httpClient.PostAsJsonAsync("/api/auth/login", request);
    }
    
    public async Task SetToken(string token)
    {
        await auth.SetTokenAsync(token);
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
    
    public async Task Logout()
    {
        await auth.ClearAsync();
        httpClient.DefaultRequestHeaders.Authorization = null;
    }
    
    public void EnsureAuthHeader()
    {
        var token = auth.Token;
        if (!string.IsNullOrWhiteSpace(token))
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
    
}