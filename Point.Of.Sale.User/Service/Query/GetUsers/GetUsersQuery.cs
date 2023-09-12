using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.User.Models;

namespace Point.Of.Sale.User.Service.Query.GetUsers;

public record GetUsersQuery : IQuery<List<ServiceUserResponse>>
{
}
