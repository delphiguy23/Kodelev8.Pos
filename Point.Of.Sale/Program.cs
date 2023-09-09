// See https://aka.ms/new-console-template for more information

using System.IO.Compression;
using System.Reflection;
using Honeycomb.OpenTelemetry;
using Honeycomb.Serilog.Sink;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Trace;
using Point.Of.Sale.Abstraction.Assembly;
using Point.Of.Sale.Category.Repository;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Initializable;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Shared.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//load appsettings configuration
builder.Services.AddOptions();
builder.Services.Configure<PosConfiguration>(builder.Configuration.GetSection("PosConfiguration"));
var options = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<PosConfiguration>>();

//setup dynamic ef providers
switch (options.Value.Database.DbProvider)
{
    case DbProvider.PostgreSql:
        builder.Services.AddDbContext<IPosDbContext, PosDbContext>(o => { o.UseNpgsql(options.Value.Database.BuildConnectionString() ?? string.Empty); });
        break;
    case DbProvider.MsSql:
        builder.Services.AddDbContext<IPosDbContext, PosDbContext>(o => { o.UseSqlServer(options.Value.Database.BuildConnectionString() ?? string.Empty); });
        break;
    case DbProvider.MySql:
        var connectionString = options.Value.Database.BuildConnectionString() ?? string.Empty;
        builder.Services.AddDbContext<IPosDbContext, PosDbContext>(o => { o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)); });
        break;
    case DbProvider.SqLlite:
        builder.Services.AddDbContext<IPosDbContext, PosDbContext>(o => { o.UseSqlite(options.Value.Database.BuildConnectionString() ?? string.Empty); });
        break;
    default:
        builder.Services.AddDbContext<IPosDbContext, PosDbContext>(o => { o.UseInMemoryDatabase(options.Value.Database.BuildConnectionString() ?? string.Empty); });
        break;
}

//setup compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });

builder.Services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });


builder.Services.AddScoped<IPosDbContext>(c => c.GetRequiredService<PosDbContext>());
builder.Services.AddScoped<IUnitOfWork>(c => c.GetRequiredService<PosDbContext>());

//register mediatr
builder.Services.AddMediatR(m => m.RegisterServicesFromAssemblies(AssemblyReference.Assembly));
builder.Services.AddMediatR(m => m.RegisterServicesFromAssemblies(Point.Of.Sale.Category.Assembly.AssemblyReference.Assembly));
builder.Services.AddMediatR(m => m.RegisterServicesFromAssemblies(Point.Of.Sale.Customer.Assembly.AssemblyReference.Assembly));
builder.Services.AddMediatR(m => m.RegisterServicesFromAssemblies(Point.Of.Sale.Inventory.Assembly.AssemblyReference.Assembly));
builder.Services.AddMediatR(m => m.RegisterServicesFromAssemblies(Point.Of.Sale.Person.Assembly.AssemblyReference.Assembly));
builder.Services.AddMediatR(m => m.RegisterServicesFromAssemblies(Point.Of.Sale.Product.Assembly.AssemblyReference.Assembly));
builder.Services.AddMediatR(m => m.RegisterServicesFromAssemblies(Point.Of.Sale.Sales.Assembly.AssemblyReference.Assembly));
builder.Services.AddMediatR(m => m.RegisterServicesFromAssemblies(Point.Of.Sale.Supplier.Assembly.AssemblyReference.Assembly));
builder.Services.AddMediatR(m => m.RegisterServicesFromAssemblies(Point.Of.Sale.Tenant.Assembly.AssemblyReference.Assembly));
builder.Services.AddMediatR(m => m.RegisterServicesFromAssemblies(Point.Of.Sale.Shopping.Cart.Assembly.AssemblyReference.Assembly));
builder.Services.AddMediatR(m => m.RegisterServicesFromAssemblies(Point.Of.Sale.Shared.Assembly.AssemblyReference.Assembly));

//scan assemblies with scrutor
builder
    .Services
    .Scan(
        selector => selector
            .FromAssemblies(AssemblyReference.Assembly)
            .FromAssemblies(Point.Of.Sale.Persistence.Assembly.AssemblyReference.Assembly)
            .FromAssemblies(Point.Of.Sale.Category.Assembly.AssemblyReference.Assembly)
            .FromAssemblies(Point.Of.Sale.Customer.Assembly.AssemblyReference.Assembly)
            .FromAssemblies(Point.Of.Sale.Inventory.Assembly.AssemblyReference.Assembly)
            .FromAssemblies(Point.Of.Sale.Person.Assembly.AssemblyReference.Assembly)
            .FromAssemblies(Point.Of.Sale.Product.Assembly.AssemblyReference.Assembly)
            .FromAssemblies(Point.Of.Sale.Sales.Assembly.AssemblyReference.Assembly)
            .FromAssemblies(Point.Of.Sale.Supplier.Assembly.AssemblyReference.Assembly)
            .FromAssemblies(Point.Of.Sale.Tenant.Assembly.AssemblyReference.Assembly)
            .FromAssemblies(Point.Of.Sale.Shopping.Cart.Assembly.AssemblyReference.Assembly)
            .FromAssemblies(Point.Of.Sale.Shared.Assembly.AssemblyReference.Assembly)
            .AddClasses(false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s => { s.SwaggerDoc("v1", new OpenApiInfo {Title = "Kodelev8 POS Service API", Version = "v1"}); });

//register repositories
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<Point.Of.Sale.Customer.Repository.IRepository, Point.Of.Sale.Customer.Repository.Repository>();
builder.Services.AddScoped<Point.Of.Sale.Inventory.Repository.IRepository, Point.Of.Sale.Inventory.Repository.Repository>();
builder.Services.AddScoped<Point.Of.Sale.Person.Repository.IRepository, Point.Of.Sale.Person.Repository.Repository>();
builder.Services.AddScoped<Point.Of.Sale.Product.Repository.IRepository, Point.Of.Sale.Product.Repository.Repository>();
builder.Services.AddScoped<Point.Of.Sale.Sales.Repository.IRepository, Point.Of.Sale.Sales.Repository.Repository>();
builder.Services.AddScoped<Point.Of.Sale.Shopping.Cart.Repository.IRepository, Point.Of.Sale.Shopping.Cart.Repository.Repository>();
builder.Services.AddScoped<Point.Of.Sale.Supplier.Repository.IRepository, Point.Of.Sale.Supplier.Repository.Repository>();
builder.Services.AddScoped<Point.Of.Sale.Tenant.Repository.IRepository, Point.Of.Sale.Tenant.Repository.Repository>();

builder.Services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("Point.Of.Sale.Category")));
builder.Services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("Point.Of.Sale.Customer")));
builder.Services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("Point.Of.Sale.Inventory")));
builder.Services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("Point.Of.Sale.Person")));
builder.Services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("Point.Of.Sale.Product")));
builder.Services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("Point.Of.Sale.Shopping.Cart")));
builder.Services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("Point.Of.Sale.Supplier")));
builder.Services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("Point.Of.Sale.Sales")));
builder.Services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("Point.Of.Sale.Tenant")));

// Setup OpenTelemetry Tracing w/ honeybcomb
HoneycombOptions honeycombOptions = new()
{
    ApiKey = options.Value.HoneyComb.ApiKey,
    Dataset = options.Value.HoneyComb.Dataset,
    ServiceName = options.Value.HoneyComb.ServiceName,
    Endpoint = options.Value.HoneyComb.Endpoint,
};

builder.Services.AddOpenTelemetry().WithTracing(otelBuilder =>
{
    otelBuilder
        .AddHoneycomb(honeycombOptions)
        .AddCommonInstrumentations();
});

builder.Services.AddSingleton(TracerProvider.Default.GetTracer(honeycombOptions.ServiceName));

using var log = new LoggerConfiguration()
    .WriteTo.HoneycombSink(options.Value.HoneyComb.Dataset, options.Value.HoneyComb.ApiKey)
    .CreateLogger();

builder.Logging.AddSerilog(log);
builder.Services.AddSingleton<ILogger>(log);


// builder.Services.AddControllers();
var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Map("/exception", () => { throw new InvalidOperationException("Sample Exception"); });

//initialize and apply database migrations
var initializables = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(y => typeof(IInitializable).IsAssignableFrom(y) && !y.IsInterface);

if (initializables is not null || initializables.Any())
{
    foreach (var init in initializables)
    {
        try
        {
            var instance = (IInitializable) Activator.CreateInstance(init)!;
            Task.Run(() => instance.Initialize()).Wait();
        }
        catch
        {
            //ignore errors
        }
    }
}

app.Run();


//json patch
//https://www.thereformedprogrammer.net/pragmatic-domain-driven-design-supporting-json-patch-in-entity-framework-core/
