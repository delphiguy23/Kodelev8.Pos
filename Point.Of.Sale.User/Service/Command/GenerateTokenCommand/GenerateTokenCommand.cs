using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.User.Service.Command.GenerateTokenCommand;

public record GenerateTokenCommand(string UserName, string Email, int TenantId) : ICommand<string>
{
}
