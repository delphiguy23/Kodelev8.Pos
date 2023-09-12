using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Models;

namespace Point.Of.Sale.User.Service.Query.UserExistQuery;

public record UserExistQuery(string UserName, string Email, int TenantId) : IQuery<ServiceUser>
{
}
