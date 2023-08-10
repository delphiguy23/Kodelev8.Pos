using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shopping.Cart.Models;

namespace Point.Of.Sale.Shopping.Cart.Service.Query.GetByTenantId;

public sealed record GetByTenantIdQuery(int id) : IQuery<List<CartResponse>>
{}
