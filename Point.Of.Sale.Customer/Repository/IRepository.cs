using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Customer.Repository;

public interface IRepository : IGenericRepository<Persistence.Models.Customer>
{
    Task<IFluentResults> LinkToTenant(LinkToTenant linkToTenant, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Persistence.Models.Customer>>> GetByTenantId(int Id, CancellationToken cancellationToken = default);
}
