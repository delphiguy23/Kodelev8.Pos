using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.User.Service.Query.ValidateUser;

public sealed record ValidateUserQuery(string UserName, string Password, int TenantId) : IQuery<bool>
{
}
