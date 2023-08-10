using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Models;

namespace Point.Of.Sale.Customer.Service.Query.GetAll;

public sealed record GetAllQuery() : IQuery<List<CustomerResponse>>
{}
