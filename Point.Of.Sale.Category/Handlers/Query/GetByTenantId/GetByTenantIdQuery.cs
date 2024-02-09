using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Models.Categories;

namespace Point.Of.Sale.Category.Handlers.Query.GetByTenantId;

public sealed record GetByTenantIdQuery(int id) : IQuery<List<CategoryResponse>>
{
}
