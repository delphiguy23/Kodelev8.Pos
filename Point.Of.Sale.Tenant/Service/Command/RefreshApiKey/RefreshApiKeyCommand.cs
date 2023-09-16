using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Tenant.Service.Command.RefreshApiKey;

public record RefreshApiKeyCommand(int TenantId, string ApiKey) : ICommand<string>
{
}
