using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Auth.Service.Command.DeleteRoleCommand;

public record DeleteRoleCommand(string RoleName) : ICommand<bool>
{
}