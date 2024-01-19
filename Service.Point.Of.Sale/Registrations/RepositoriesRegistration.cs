using Microsoft.Extensions.DependencyInjection;
using Point.Of.Sale.Category.Repository;

namespace Service.Point.Of.Sale.Registrations;

public static class RepositoriesRegistration
{
    public static void AddRepositoriesRegistration(this IServiceCollection services)
    {
        services.AddScoped<IRepository, Repository>();
        services
            .AddScoped<global::Point.Of.Sale.Customer.Repository.IRepository,
                global::Point.Of.Sale.Customer.Repository.Repository>();
        services
            .AddScoped<global::Point.Of.Sale.Inventory.Repository.IRepository,
                global::Point.Of.Sale.Inventory.Repository.Repository>();
        // services
        //     .AddScoped<global::Point.Of.Sale.Person.Repository.IRepository,
        //         global::Point.Of.Sale.Person.Repository.Repository>();
        services
            .AddScoped<global::Point.Of.Sale.Product.Repository.IRepository,
                global::Point.Of.Sale.Product.Repository.Repository>();
        services
            .AddScoped<global::Point.Of.Sale.Sales.Repository.IRepository,
                global::Point.Of.Sale.Sales.Repository.Repository>();
        services
            .AddScoped<global::Point.Of.Sale.Shopping.Cart.Repository.IRepository,
                global::Point.Of.Sale.Shopping.Cart.Repository.Repository>();
        services
            .AddScoped<global::Point.Of.Sale.Supplier.Repository.IRepository,
                global::Point.Of.Sale.Supplier.Repository.Repository>();
        services
            .AddScoped<global::Point.Of.Sale.Tenant.Repository.IRepository,
                global::Point.Of.Sale.Tenant.Repository.Repository>();
        services
            .AddScoped<global::Point.Of.Sale.Events.Repository.IRepository,
                global::Point.Of.Sale.Events.Repository.Repository>();
    }
}
