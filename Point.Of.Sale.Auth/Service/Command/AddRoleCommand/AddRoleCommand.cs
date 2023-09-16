using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Auth.Service.Command.AddRoleCommand;

public record AddRoleCommand(string Role) : ICommand<bool>
{
}