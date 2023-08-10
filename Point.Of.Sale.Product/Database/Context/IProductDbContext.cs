using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Initializable;

namespace Point.Of.Sale.Product.Database.Context;

public interface IProductDbContext: IDbContext, IInitializable
{
    DbSet<Model.Product> Products { get; set; }
}
