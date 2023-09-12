using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.User.Service.Command.DeleteUserCommand;

public record DeleteUserCommand(string UserName) : ICommand<bool>
{
}
