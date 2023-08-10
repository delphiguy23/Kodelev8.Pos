using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Tenant.Models;

namespace Point.Of.Sale.Tenant.Service.Query.GetAllTenants;

public sealed record GetAll () : IQuery<List<TenantResponse>>
{
}
