using Microsoft.Extensions.DependencyInjection;
using Point.Of.Sale.Abstraction.Assembly;

namespace Point.Of.Sale.Registrations;

public static class ScrutorRegistration
{
    public static void AddScrutorRegistration(this IServiceCollection service)
    {
        service.Scan(
            selector => selector
                .FromAssemblies(AssemblyReference.Assembly)
                .FromAssemblies(Persistence.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Category.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Customer.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Inventory.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Person.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Product.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Sales.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Supplier.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Tenant.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Shopping.Cart.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Auth.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Shared.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Events.Assembly.AssemblyReference.Assembly)
                .AddClasses(false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}
