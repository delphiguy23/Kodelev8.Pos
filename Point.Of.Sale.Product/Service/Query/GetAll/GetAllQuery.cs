using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Product.Models;

namespace Point.Of.Sale.Product.Service.Query.GetAll;

public sealed record GetAllQuery() : IQuery<List<ProductResponse>>
{}
