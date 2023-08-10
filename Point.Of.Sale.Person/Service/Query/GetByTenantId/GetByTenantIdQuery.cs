using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Person.Models;

namespace Point.Of.Sale.Person.Service.Query.GetByTenantId;

public sealed record GetByTenantIdQuery(int id) : IQuery<List<PersonResponse>>
{}
