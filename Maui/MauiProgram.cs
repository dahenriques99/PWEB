using Microsoft.Extensions.Logging;
using RCL.Components.Services;

namespace Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();
        
        builder.Services.AddHttpClient<RestService>(client =>
        {
            var baseUrl =
                DeviceInfo.Platform == DevicePlatform.Android
                    ? "http://10.0.2.2:7124/"   // only if API is HTTP
                    : "https://localhost:7124/";

            client.BaseAddress = new Uri(baseUrl);
        });
        
        builder.Services.AddScoped<ITokenStore, MauiSecureTokenStore>();
        
        builder.Services.AddScoped<AuthStateService>();
        builder.Services.AddScoped<CartService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}