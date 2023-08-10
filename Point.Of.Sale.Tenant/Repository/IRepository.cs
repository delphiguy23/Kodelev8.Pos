using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Models;

namespace Point.Of.Sale.Tenant.Repository;

public interface IRepository
{
    Task<IFluentResults<List<Database.Model.Tenant>>> All (CancellationToken cancellationToken = default);
    Task<IFluentResults<Database.Model.Tenant>> GetById (int request, CancellationToken cancellationToken = default);
    Task<IFluentResults> Add (UpsertTenant request, CancellationToken cancellationToken = default);
    Task<IFluentResults> Update (UpsertTenant request, CancellationToken cancellationToken = default);
}
