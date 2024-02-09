using MudBlazor.Extensions;
using MudBlazor.Services;
using MudExtensions.Services;
using Web.UI.Point.Of.Sale.Components;
using Web.UI.Point.Of.Sale.Services.Auth;
using Web.UI.Point.Of.Sale.Services.Tenant;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("Kodelev8-POS", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITenantService, TenantService>();

builder.Services.AddMudServices();
builder.Services.AddMudExtensions();
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
    .AddInteractiveServerRenderMode();


app.Run();
