using Point.Of.Sale.Inventory.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Inventory.Repository;

public interface IRepository
{
    Task<IFluentResults<List<Sale.Inventory.Database.Model.Inventory>>> All (CancellationToken cancellationToken = default);
    Task<IFluentResults<Sale.Inventory.Database.Model.Inventory>> GetById (int Id, CancellationToken cancellationToken = default);
    Task<IFluentResults> Add (UpsertInventory request, CancellationToken cancellationToken = default);
    Task<IFluentResults> LinkToTenant (LinkToTenant linkToTenant, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Sale.Inventory.Database.Model.Inventory>>> GetByTenantId (int Id, CancellationToken cancellationToken = default);
    Task<IFluentResults> Update (UpsertInventory request, CancellationToken cancellationToken = default);
}
