using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Initializable;

namespace Point.Of.Sale.Supplier.Database.Context;

public interface ISupplierDbContext: IDbContext, IInitializable
{
    DbSet<Model.Supplier> Suppliers { get; set; }
}
