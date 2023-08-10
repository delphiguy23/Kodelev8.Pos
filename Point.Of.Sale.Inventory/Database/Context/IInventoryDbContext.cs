using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Initializable;

namespace Point.Of.Sale.Inventory.Database.Context;

public interface IInventoryDbContext: IDbContext, IInitializable
{
    DbSet<Model.Inventory> Inventories { get; set; }
}
