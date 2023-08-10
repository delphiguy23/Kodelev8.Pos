using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Person.Models;

namespace Point.Of.Sale.Person.Service.Query.GetAll;

public sealed record GetAllQuery() : IQuery<List<PersonResponse>>
{}
