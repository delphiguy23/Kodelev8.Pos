using Point.Of.Sale.Customer.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Customer.Repository;

public interface IRepository
{
    Task<IFluentResults<List<Database.Model.Customer>>> All (CancellationToken cancellationToken = default);
    Task<IFluentResults<Database.Model.Customer>> GetById (int Id, CancellationToken cancellationToken = default);
    Task<IFluentResults> Add (AddCustomer newCustomer, CancellationToken cancellationToken = default);
    Task<IFluentResults> LinkToTenant (LinkToTenant linkToTenant, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Database.Model.Customer>>> GetByTenantId (int Id, CancellationToken cancellationToken = default);
    Task<IFluentResults> Update (UpdateCustomer updateCustomer, CancellationToken cancellationToken = default);
}
