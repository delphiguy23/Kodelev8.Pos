using Point.Of.Sale.Sales.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Sales.Repository;

public interface IRepository
{
    Task<IFluentResults<List<Sale.Sales.Database.Model.Sale>>> All (CancellationToken cancellationToken = default);
    Task<IFluentResults<Sale.Sales.Database.Model.Sale>> GetById (int Id, CancellationToken cancellationToken = default);
    Task<IFluentResults> Add (UpsertSale newSale, CancellationToken cancellationToken = default);
    Task<IFluentResults> LinkToTenant (LinkToTenant linkToTenant, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Sale.Sales.Database.Model.Sale>>> GetByTenantId (int Id, CancellationToken cancellationToken = default);
    Task<IFluentResults> Update (UpsertSale updateCustomer, CancellationToken cancellationToken = default);
    Task<IFluentResults> UpsertLineItem (UpsertSaleLineItem request, CancellationToken cancellationToken = default);
}
