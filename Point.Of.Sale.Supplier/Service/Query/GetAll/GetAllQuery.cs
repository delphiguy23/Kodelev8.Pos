using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Supplier.Models;

namespace Point.Of.Sale.Supplier.Service.Query.GetAll;

public sealed record GetAllQuery() : IQuery<List<SupplierResponse>>
{}
