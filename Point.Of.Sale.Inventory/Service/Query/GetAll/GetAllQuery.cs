using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Inventory.Models;

namespace Point.Of.Sale.Inventory.Service.Query.GetAll;

public sealed record GetAllQuery() : IQuery<List<InventoryResponse>>
{}
