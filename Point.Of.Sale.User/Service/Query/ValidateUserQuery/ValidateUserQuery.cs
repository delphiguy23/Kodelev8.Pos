using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.User.Service.Query.ValidateUserQuery;

public sealed record ValidateUserQuery(string UserName, string Password, string Email, int TenantId) : IQuery<bool>
{
}
