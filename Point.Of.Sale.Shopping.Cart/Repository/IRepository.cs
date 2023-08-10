using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;
using Point.Of.Sale.Shopping.Cart.Models;

namespace Point.Of.Sale.Shopping.Cart.Repository;

public interface IRepository
{
    Task<IFluentResults<List<Shopping.Cart.Database.Model.ShoppingCart>>> All (CancellationToken cancellationToken = default);
    Task<IFluentResults<Shopping.Cart.Database.Model.ShoppingCart>> GetById (int request, CancellationToken cancellationToken = default);
    Task<IFluentResults> Add (UpsertCart newCart, CancellationToken cancellationToken = default);
    Task<IFluentResults> LinkToTenant (LinkToTenant request, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Shopping.Cart.Database.Model.ShoppingCart>>> GetByTenantId (int request, CancellationToken cancellationToken = default);
    Task<IFluentResults> Update (UpsertCart request, CancellationToken cancellationToken = default);
}
