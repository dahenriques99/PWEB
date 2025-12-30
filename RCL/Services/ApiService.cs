using System.Net.Http.Json;
using RCL.Dtos;

namespace RCL.Services;

public class ApiService(HttpClient httpClient)
{
    public async Task<IEnumerable<ProductCategoryDto>?> GetAllProductsWithCategories()
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<IEnumerable<ProductCategoryDto>>("api/ProductCategory/products");
            return response;
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Error fetching products with categories: {ex.Message}");
            return Enumerable.Empty<ProductCategoryDto>();
        }
    }

    public async Task<IEnumerable<CategoryDto>?> GetAllCategories()
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<IEnumerable<CategoryDto>>("api/Category");
            return response;
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Error fetching categories: {ex.Message}");
            return Enumerable.Empty<CategoryDto>();
        }
    }

    public async Task<IEnumerable<SupplierDto>?> GetAllCompanies()
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<IEnumerable<SupplierDto>>("api/Company");
            return response;
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Error fetching companies: {ex.Message}");
            return Enumerable.Empty<SupplierDto>();
        }
    }

    public async Task AddToCartAsync(CartDto dto, string token)
    {
        try
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.PostAsJsonAsync("api/Order/cart", dto);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Error adding product to cart: {ex.Message}");
            throw;
        }
    }

    public async Task<UserDto?> GetUserInfo(string email, string token)
    {
        try
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.GetFromJsonAsync<UserDto>($"api/User/email/{email}");
            return response;
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Error fetching client info: {ex.Message}");
            return null;
        }
    }
}