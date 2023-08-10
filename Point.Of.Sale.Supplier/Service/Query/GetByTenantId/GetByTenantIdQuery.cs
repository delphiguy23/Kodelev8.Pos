using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Supplier.Models;

namespace Point.Of.Sale.Supplier.Service.Query.GetByTenantId;

public sealed record GetByTenantIdQuery(int id) : IQuery<List<SupplierResponse>>
{}
