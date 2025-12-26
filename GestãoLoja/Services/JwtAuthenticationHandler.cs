using System.Net.Http.Headers;

namespace GestãoLoja.Services;

public class JwtAuthenticationHandler(TokenStorageService tokenStorageService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = tokenStorageService.GetToken();

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Send request with the token (if it exists)
        return await base.SendAsync(request, cancellationToken);
    }
}