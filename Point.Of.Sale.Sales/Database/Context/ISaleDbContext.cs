using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Initializable;

namespace Point.Of.Sale.Sales.Database.Context;

public interface ISaleDbContext: IDbContext, IInitializable
{
    DbSet<Model.Sale> Sales { get; set; }
}
