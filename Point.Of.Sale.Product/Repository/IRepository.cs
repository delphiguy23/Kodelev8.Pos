using Point.Of.Sale.Product.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Product.Repository;

public interface IRepository
{
    Task<IFluentResults<List<Database.Model.Product>>> All (CancellationToken cancellationToken = default);
    Task<IFluentResults<Database.Model.Product>> GetById (int Id, CancellationToken cancellationToken = default);
    Task<IFluentResults> Add (UpsertProduct newProduct, CancellationToken cancellationToken = default);
    Task<IFluentResults> LinkToTenant (LinkToTenant linkToTenant, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Database.Model.Product>>> GetByTenantId (int Id, CancellationToken cancellationToken = default);
    Task<IFluentResults> Update (UpsertProduct updateCustomer, CancellationToken cancellationToken = default);

}
