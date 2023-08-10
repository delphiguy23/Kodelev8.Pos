using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Initializable;

namespace Point.Of.Sale.Category.Database.Context;

public interface ICategoryDbContext : IDbContext, IInitializable
{
    DbSet<Model.Category> Categories { get; set; }
}
