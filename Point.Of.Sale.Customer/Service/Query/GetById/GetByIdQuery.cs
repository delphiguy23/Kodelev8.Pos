using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Models;

namespace Point.Of.Sale.Customer.Service.Query.GetById;

public sealed record GetById (int Id) : IQuery<CustomerResponse>
{
}
