using Web.Wasm.Point.Of.Sale.Services.Enum;

namespace Web.Wasm.Point.Of.Sale.Services.Tenant.Models;

public record UpsertTenant
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public TenantType Type { get; set; }
    public string Email { get; set; }
    public bool Active { get; set; }
}
