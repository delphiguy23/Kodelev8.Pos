using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.User.Service.Command.DeleteRoleCommand;

public record DeleteRoleCommand(string RoleName) : ICommand<bool>
{
}
