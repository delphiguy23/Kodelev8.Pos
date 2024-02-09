using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MudExtensions.Services;
using Web.Wasm.Point.Of.Sale;
using Web.Wasm.Point.Of.Sale.Services.Supplier;
using Web.Wasm.Point.Of.Sale.Services.Tenant;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddMudExtensions();

// builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000") });

// builder.Services.AddHttpClient("Kodelev8-POS", client =>
// {
//     client.BaseAddress = new Uri("http://localhost:5000");
//     client.DefaultRequestHeaders.Add("Accept", "application/json");
// });

// builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
// {
//     builder.AllowAnyOrigin()
//         .AllowAnyMethod()
//         .AllowAnyHeader();
// }));


await builder.Build().RunAsync();
