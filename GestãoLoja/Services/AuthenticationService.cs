using System.Text.Json;

namespace GestãoLoja.Services;

public class AuthenticationService(IHttpClientFactory httpFactory, TokenStorageService tokenStorageService)
{
     private readonly HttpClient _http = httpFactory.CreateClient("api");

     public async Task<LoginResult> Login(string email, string password)
    {
        try
        {
            var loginData = new
            {
                Email = email,
                Password = password
            };
            //var result = await _http.PostAsJsonAsync("/Identity/Login", loginData);
            var result = await _http.PostAsJsonAsync("/api/auth/login", loginData);

            if (result.IsSuccessStatusCode)
            {
                // Deserialize JSON response from API
                var json = await result.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                if (authResponse != null && !string.IsNullOrEmpty(authResponse.AccessToken))
                {
                    tokenStorageService.SetToken(authResponse.AccessToken, authResponse.ExpiresIn);
                }
                
                Console.WriteLine("---------------------------");
                Console.WriteLine("Login successful!");
                Console.WriteLine("Token received: " + authResponse?.AccessToken.Substring(20) + "...");
                Console.WriteLine("Token Type: {0}", authResponse?.TokenType);
                Console.WriteLine("Expires in (seconds): {0}", authResponse.ExpiresIn);
                Console.WriteLine("Email: {0}", authResponse.Email);
                Console.WriteLine("---------------------------");

                return new LoginResult
                {
                    Success = true,
                    Message = "Login successful!",
                    Email = authResponse.Email
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Login failed: " + ex.Message);
            return new LoginResult
            {
                Success = false,
                Message = "Login failed: " + ex.Message
            };
        }
        return new LoginResult
        {
            Success = false,
            Message = "Login failed:"
        };
    }
}