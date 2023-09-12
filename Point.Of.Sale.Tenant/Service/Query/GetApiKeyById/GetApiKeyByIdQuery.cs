using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Tenant.Service.Query.GetApiKeyById;

public sealed record GetApiKeyByIdQuery(int Id) : IQuery<string>
{
}
