using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Auth.Service.Command.GenerateTokenCommand;

public record GenerateTokenCommand(string UserName, string Email, int TenantId) : ICommand<string>
{
}