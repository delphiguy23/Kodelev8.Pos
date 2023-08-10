using Point.Of.Sale.Shared.Enums;

namespace Point.Of.Sale.Tenant.Database.Model;

public class Tenant
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public TenantType Type { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string CreatedBy { get; set; }
}
