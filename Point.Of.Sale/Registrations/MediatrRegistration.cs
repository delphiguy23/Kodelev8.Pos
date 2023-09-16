using Microsoft.Extensions.DependencyInjection;
using Point.Of.Sale.Abstraction.Assembly;

namespace Point.Of.Sale.Registrations;

public static class MediatrRegistration
{
    public static void AddMediatrRegistration(this IServiceCollection services)
    {
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Category.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Customer.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Inventory.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Person.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Product.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Sales.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Supplier.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Tenant.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Shopping.Cart.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Shared.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Auth.Assembly.AssemblyReference.Assembly));
    }
}
