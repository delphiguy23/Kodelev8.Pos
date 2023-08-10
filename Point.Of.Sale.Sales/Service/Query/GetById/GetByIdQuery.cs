using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Sales.Models;

namespace Point.Of.Sale.Sales.Service.Query.GetById;

public sealed record GetById (int Id) : IQuery<SaleResponse>;
