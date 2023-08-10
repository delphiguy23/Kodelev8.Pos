using Point.Of.Sale.Shared.Enums;

namespace Point.Of.Sale.Db.Views.Database.Model;

public class ProductView
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string TenantName { get; set; }
    public TenantType Type { get; set; }
    public string SkuCode { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public decimal UnitPrice { get; set; }
    public int SupplierId { get; set; }
    public string SupplierName { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string UpdatedBy { get; set; }
}
