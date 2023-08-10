using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Supplier.Models;

namespace Point.Of.Sale.Supplier.Service.Query.GetById;

public sealed record GetById (int Id) : IQuery<SupplierResponse>
{
}
