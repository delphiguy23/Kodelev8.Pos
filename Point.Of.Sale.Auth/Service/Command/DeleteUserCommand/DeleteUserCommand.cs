using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Auth.Service.Command.DeleteUserCommand;

public record DeleteUserCommand(string UserName) : ICommand<bool>
{
}