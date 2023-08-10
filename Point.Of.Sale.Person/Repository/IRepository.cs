using Point.Of.Sale.Person.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Person.Repository;

public interface IRepository
{
    Task<IFluentResults<List<Sale.Person.Database.Model.Person>>> All (CancellationToken cancellationToken = default);
    Task<IFluentResults<Sale.Person.Database.Model.Person>> GetById (int Id, CancellationToken cancellationToken = default);
    Task<IFluentResults> AddPerson (UpsertPerson newPerson, CancellationToken cancellationToken = default);
    Task<IFluentResults> LinkToTenant (LinkToTenant linkToTenant, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Sale.Person.Database.Model.Person>>> GetByTenantId (int Id, CancellationToken cancellationToken = default);
    Task<IFluentResults> Update (UpsertPerson updateCustomer, CancellationToken cancellationToken = default);
}
