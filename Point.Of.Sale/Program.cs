// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Point.Of.Sale.Assembly;
using Point.Of.Sale.Category.Database.Context;
using Point.Of.Sale.Customer.Database.Context;
using Point.Of.Sale.Inventory.Database.Context;
using Point.Of.Sale.Persistence.Initializable;
using Point.Of.Sale.Person.Database.Context;
using Point.Of.Sale.Product.Database.Context;
using Point.Of.Sale.Sales.Database.Context;
using Point.Of.Sale.Shopping.Cart.Database.Context;
using Point.Of.Sale.Supplier.Database.Context;
using Point.Of.Sale.Tenant.Database.Context;

const string url = "https://ykoorfkswtiuzwokviis.supabase.co"; // Environment.GetEnvironmentVariable("SUPABASE_URL");
const string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Inlrb29yZmtzd3RpdXp3b2t2aWlzIiwicm9sZSI6ImFub24iLCJpYXQiOjE2OTAwOTM1NzEsImV4cCI6MjAwNTY2OTU3MX0.2hIJJvz91kRAPWjg6KcIu-UhzuJEUOc5jIiArRGyxIY"; //Environment.GetEnvironmentVariable("SUPABASE_KEY");

var builder = WebApplication.CreateBuilder(args);

var connectionString = "User Id=postgres;Password=xqdOSyXTk69227f5;Server=db.ykoorfkswtiuzwokviis.supabase.co;Port=5432;Database=postgres";

builder.Services.AddDbContext<ICategoryDbContext, CategoryDbContext>(o => { o.UseNpgsql(connectionString); });
builder.Services.AddDbContext<ICustomerDbContext, CustomerDbContext>(o => { o.UseNpgsql(connectionString); });
builder.Services.AddDbContext<IInventoryDbContext, InventoryDbContext>(o => { o.UseNpgsql(connectionString); });
builder.Services.AddDbContext<IPersonDbContext, PersonDbContext>(o => { o.UseNpgsql(connectionString); });
builder.Services.AddDbContext<IProductDbContext, ProductDbContext>(o => { o.UseNpgsql(connectionString); });
builder.Services.AddDbContext<ISaleDbContext, SaleDbContext>(o => { o.UseNpgsql(connectionString); });
builder.Services.AddDbContext<IShoppingCartDbContext, ShoppingCartDbContext>(o => { o.UseNpgsql(connectionString); });
builder.Services.AddDbContext<ISupplierDbContext, SupplierDbContext>(o => { o.UseNpgsql(connectionString); });
builder.Services.AddDbContext<ITenantDbContext, TenantDbContext>(o => { o.UseNpgsql(connectionString); });

builder.Services.AddMediatR(m => m.RegisterServicesFromAssemblies(Point.Of.Sale.Abstraction.Assembly.AssemblyReference.Assembly));
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

builder
    .Services
    .Scan(
        selector => selector
            .FromAssemblies(Point.Of.Sale.Abstraction.Assembly.AssemblyReference.Assembly)
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


var options = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true,
    AutoRefreshToken = true,
};

var supabase = new Supabase.Client(url, key, options);

await supabase.InitializeAsync();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new() {Title = "Kodelev8 POS Service API", Version = "v1"});
});

builder.Services.AddScoped<Point.Of.Sale.Category.Repository.IRepository, Point.Of.Sale.Category.Repository.Repository>();
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

// builder.Services.AddMediatR(m => m.RegisterServicesFromAssemblyContaining<Program>());

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


// app.RegisterUserManagementEndpoints(supabase);

app.Map("/exception", () => { throw new InvalidOperationException("Sample Exception"); });

IEnumerable<Type> initializables = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(y => typeof(IInitializable).IsAssignableFrom(y) && !y.IsInterface);

if (initializables is not null || initializables.Any())
{
    // var initializablesInstances = new List<IInitializable>();

    foreach (var init in initializables)
    {
        try
        {
            var instance = (IInitializable) Activator.CreateInstance(init)!;
            Task.Run(() => instance.Initialize()).Wait();
            // initializablesInstances.Add(instance);
        }
        catch
        {
            //ignore errors
        }
    }

    //execute in parallel
    //cant execute in porallel, due to race condition in populating __EFMigrations table
    // Task.WaitAll(initializablesInstances.Select(x => x.Initialize()).ToArray());
}

app.Run();
