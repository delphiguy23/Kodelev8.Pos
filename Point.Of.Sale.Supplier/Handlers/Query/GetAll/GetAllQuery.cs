using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Models.Supplier;

namespace Point.Of.Sale.Supplier.Handlers.Query.GetAll;

public sealed record GetAllQuery : IQuery<List<SupplierResponse>>
{
}