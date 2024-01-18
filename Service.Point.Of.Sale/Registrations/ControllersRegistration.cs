using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Service.Point.Of.Sale.Registrations;

public static class ControllersRegistration
{
    public static void AddControllersRegistration(this IServiceCollection services)
    {
        services.AddMvc()
            .AddApplicationPart(System.Reflection.Assembly.Load(new AssemblyName("Point.Of.Sale.Category")));
        services.AddMvc()
            .AddApplicationPart(System.Reflection.Assembly.Load(new AssemblyName("Point.Of.Sale.Customer")));
        services.AddMvc()
            .AddApplicationPart(System.Reflection.Assembly.Load(new AssemblyName("Point.Of.Sale.Inventory")));
        // services.AddMvc()
        //     .AddApplicationPart(System.Reflection.Assembly.Load(new AssemblyName("Point.Of.Sale.Person")));
        services.AddMvc()
            .AddApplicationPart(System.Reflection.Assembly.Load(new AssemblyName("Point.Of.Sale.Product")));
        services.AddMvc()
            .AddApplicationPart(
                System.Reflection.Assembly.Load(new AssemblyName("Point.Of.Sale.Shopping.Cart")));
        services.AddMvc()
            .AddApplicationPart(System.Reflection.Assembly.Load(new AssemblyName("Point.Of.Sale.Supplier")));
        services.AddMvc()
            .AddApplicationPart(System.Reflection.Assembly.Load(new AssemblyName("Point.Of.Sale.Sales")));
        services.AddMvc()
            .AddApplicationPart(System.Reflection.Assembly.Load(new AssemblyName("Point.Of.Sale.Tenant")));
        services.AddMvc()
            .AddApplicationPart(System.Reflection.Assembly.Load(new AssemblyName("Point.Of.Sale.Auth")));
    }
}
