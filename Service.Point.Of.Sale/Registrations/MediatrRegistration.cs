using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Point.Of.Sale.Abstraction.Assembly;
using Point.Of.Sale.Events.Behaviours;

namespace Service.Point.Of.Sale.Registrations;

public static class MediatrRegistration
{
    public static void AddMediatrRegistration(this IServiceCollection services)
    {
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(AssemblyReference.Assembly));
        services.AddMediatR(m =>
            m.RegisterServicesFromAssemblies(global::Point.Of.Sale.Category.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m =>
            m.RegisterServicesFromAssemblies(global::Point.Of.Sale.Customer.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m =>
            m.RegisterServicesFromAssemblies(global::Point.Of.Sale.Inventory.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m =>
            m.RegisterServicesFromAssemblies(global::Point.Of.Sale.Person.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m =>
            m.RegisterServicesFromAssemblies(global::Point.Of.Sale.Product.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m =>
            m.RegisterServicesFromAssemblies(global::Point.Of.Sale.Sales.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m =>
            m.RegisterServicesFromAssemblies(global::Point.Of.Sale.Supplier.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m =>
            m.RegisterServicesFromAssemblies(global::Point.Of.Sale.Tenant.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m =>
            m.RegisterServicesFromAssemblies(global::Point.Of.Sale.Shopping.Cart.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m =>
            m.RegisterServicesFromAssemblies(global::Point.Of.Sale.Shared.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m =>
            m.RegisterServicesFromAssemblies(global::Point.Of.Sale.Auth.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m =>
            m.RegisterServicesFromAssemblies(global::Point.Of.Sale.Events.Assembly.AssemblyReference.Assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    }
}