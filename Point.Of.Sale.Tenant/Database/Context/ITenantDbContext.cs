using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Initializable;

namespace Point.Of.Sale.Tenant.Database.Context;

public interface ITenantDbContext: IDbContext, IInitializable
{
    DbSet<Model.Tenant> Tenants { get; set; }
}
