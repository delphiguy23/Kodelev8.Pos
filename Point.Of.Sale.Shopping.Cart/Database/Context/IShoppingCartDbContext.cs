using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Initializable;

namespace Point.Of.Sale.Shopping.Cart.Database.Context;

public interface IShoppingCartDbContext: IDbContext, IInitializable
{
    DbSet<Model.ShoppingCart> ShoppingCarts { get; set; }
}
