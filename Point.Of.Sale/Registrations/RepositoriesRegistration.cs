using Microsoft.Extensions.DependencyInjection;
using Point.Of.Sale.Category.Repository;

namespace Point.Of.Sale.Registrations;

public static class epositoriesRegistration
{
    public static void AddRepositoriesRegistration(this IServiceCollection services)
    {
        services.AddScoped<IRepository, Repository>();
        services.AddScoped<Customer.Repository.IRepository, Customer.Repository.Repository>();
        services.AddScoped<Inventory.Repository.IRepository, Inventory.Repository.Repository>();
        services.AddScoped<Person.Repository.IRepository, Person.Repository.Repository>();
        services.AddScoped<Product.Repository.IRepository, Product.Repository.Repository>();
        services.AddScoped<Sales.Repository.IRepository, Sales.Repository.Repository>();
        services.AddScoped<Shopping.Cart.Repository.IRepository, Shopping.Cart.Repository.Repository>();
        services.AddScoped<Supplier.Repository.IRepository, Supplier.Repository.Repository>();
        services.AddScoped<Tenant.Repository.IRepository, Tenant.Repository.Repository>();
        services.AddScoped<Events.Repository.IRepository, Events.Repository.Repository>();
    }
}
