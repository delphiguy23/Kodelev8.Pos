using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Product.Models;

namespace Point.Of.Sale.Product.Service.Query.GetById;

public sealed record GetById (int Id) : IQuery<ProductResponse>
{
}
