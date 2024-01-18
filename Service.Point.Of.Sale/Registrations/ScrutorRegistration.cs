using Microsoft.Extensions.DependencyInjection;
using Point.Of.Sale.Abstraction.Assembly;

namespace Service.Point.Of.Sale.Registrations;

public static class ScrutorRegistration
{
    public static void AddScrutorRegistration(this IServiceCollection service)
    {
        service.Scan(
            selector => selector
                .FromAssemblies(AssemblyReference.Assembly)
                .FromAssemblies(global::Point.Of.Sale.Persistence.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(global::Point.Of.Sale.Category.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(global::Point.Of.Sale.Customer.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(global::Point.Of.Sale.Inventory.Assembly.AssemblyReference.Assembly)
                // .FromAssemblies(global::Point.Of.Sale.Person.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(global::Point.Of.Sale.Product.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(global::Point.Of.Sale.Sales.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(global::Point.Of.Sale.Supplier.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(global::Point.Of.Sale.Tenant.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(global::Point.Of.Sale.Shopping.Cart.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(global::Point.Of.Sale.Auth.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(global::Point.Of.Sale.Shared.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(global::Point.Of.Sale.Events.Assembly.AssemblyReference.Assembly)
                .AddClasses(false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}
