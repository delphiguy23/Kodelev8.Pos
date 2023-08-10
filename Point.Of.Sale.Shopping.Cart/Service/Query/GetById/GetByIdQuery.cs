using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shopping.Cart.Models;

namespace Point.Of.Sale.Shopping.Cart.Service.Query.GetById;

public sealed record GetById (int Id) : IQuery<CartResponse>
{
}
