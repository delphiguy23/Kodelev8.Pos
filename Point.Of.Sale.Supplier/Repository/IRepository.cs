using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;
using Point.Of.Sale.Supplier.Models;

namespace Point.Of.Sale.Supplier.Repository;

public interface IRepository
{
    Task<IFluentResults<List<Database.Model.Supplier>>> All (CancellationToken cancellationToken = default);
    Task<IFluentResults<Database.Model.Supplier>> GetById (int request, CancellationToken cancellationToken = default);
    Task<IFluentResults> Add (UpsertSupplier request, CancellationToken cancellationToken = default);
    Task<IFluentResults> LinkToTenant (LinkToTenant request, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Database.Model.Supplier>>> GetByTenantId (int request, CancellationToken cancellationToken = default);
    Task<IFluentResults> Update (UpsertSupplier request, CancellationToken cancellationToken = default);
}
