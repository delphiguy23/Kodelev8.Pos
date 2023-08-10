using Point.Of.Sale.Category.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Category.Repository;

public interface IRepository
{
    Task<IFluentResults<List<Database.Model.Category>>> All (CancellationToken cancellationToken = default);
    Task<IFluentResults<Database.Model.Category>> GetById (int Id, CancellationToken cancellationToken = default);
    Task<IFluentResults> Add (AddCategory newCategory, CancellationToken cancellationToken = default);
    Task<IFluentResults> LinkToTenant (LinkToTenant linkToTenant, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Database.Model.Category>>> GetByTenantId (int Id, CancellationToken cancellationToken = default);
    Task<IFluentResults> Update (UpdateCategory updateCategory, CancellationToken cancellationToken = default);
}
