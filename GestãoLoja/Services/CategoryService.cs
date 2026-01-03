using MyMedia.Infrastructure.Entities;
namespace GestãoLoja.Services;

public class CategoryService(IHttpClientFactory factory)
{
    private readonly HttpClient _http = factory.CreateClient("api");

    public async Task<IEnumerable<Category>?> GetCategories()
    {
        return await _http.GetFromJsonAsync<IEnumerable<Category>>("api/Categories");
    }

    public async Task<Category?> PostCategoria(Category categoria)
    {
        var response = await _http.PostAsJsonAsync("api/Categories", categoria);
        //Console.WriteLine(response);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<Category>();
    }

}