using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Supplier.Service.Command.LinkToTenant;

public sealed record LinkToTenantCommand(int tenantId, int entityId) : ICommand;
