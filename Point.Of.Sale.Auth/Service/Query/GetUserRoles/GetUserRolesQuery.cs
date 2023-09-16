using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Auth.Service.Query.GetUserRoles;

public record GetUserRolesQuery(string UserName) : IQuery<List<string>>
{
}