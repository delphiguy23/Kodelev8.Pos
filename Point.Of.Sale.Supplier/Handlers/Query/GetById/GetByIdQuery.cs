using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Models.Supplier;

namespace Point.Of.Sale.Supplier.Handlers.Query.GetById;

public sealed record GetById(int Id) : IQuery<SupplierResponse>
{
}