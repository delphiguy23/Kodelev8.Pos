using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Person.Models;

namespace Point.Of.Sale.Person.Service.Query.GetById;

public sealed record GetPersonById (int Id) : IQuery<PersonResponse>
{
}
