using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Inventory.Models;

namespace Point.Of.Sale.Inventory.Service.Query.GetByTenantId;

public sealed record GetByTenantIdQuery(int id) : IQuery<List<InventoryResponse>>
{}
