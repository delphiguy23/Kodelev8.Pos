using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Inventory.Repository;

public interface IRepository : IGenericRepository<Persistence.Models.Inventory>
{
    Task<IFluentResults> LinkToTenant(LinkToTenant linkToTenant, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Persistence.Models.Inventory>>> GetByTenantId(int Id, CancellationToken cancellationToken = default);
}
