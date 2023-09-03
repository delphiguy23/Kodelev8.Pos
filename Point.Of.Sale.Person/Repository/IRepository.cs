using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Person.Repository;

public interface IRepository : IGenericRepository<Persistence.Models.Person>
{
    Task<IFluentResults> LinkToTenant(LinkToTenant linkToTenant, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Persistence.Models.Person>>> GetByTenantId(int Id, CancellationToken cancellationToken = default);
}
