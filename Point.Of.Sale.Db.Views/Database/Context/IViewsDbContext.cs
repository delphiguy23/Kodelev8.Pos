using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Initializable;

namespace Point.Of.Sale.Db.Views.Database.Context;

public interface IViewsDbContext : IDbContext, IInitializable
{
    DbSet<Model.ProductView> ProductsView { get; set; }
    DbSet<Model.InventoryView> InventoriesView { get; set; }
}
