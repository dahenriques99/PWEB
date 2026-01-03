using BlazorWeb.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using RCL.Components.Services;
using RCL.Pages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<RestService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7124/");
});

builder.Services.AddScoped<ProtectedSessionStorage>();

builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<CartService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddAdditionalAssemblies(typeof(Home).Assembly)
    .AddInteractiveServerRenderMode();

app.Run();