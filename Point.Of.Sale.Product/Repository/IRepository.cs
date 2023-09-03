using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Product.Repository;

public interface IRepository : IGenericRepository<Persistence.Models.Product>
{
    Task<IFluentResults> LinkToTenant(LinkToTenant linkToTenant, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Persistence.Models.Product>>> GetByTenantId(int id, CancellationToken cancellationToken = default);
}
