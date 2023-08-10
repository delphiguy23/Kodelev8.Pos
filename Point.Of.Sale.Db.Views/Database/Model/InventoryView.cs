namespace Point.Of.Sale.Db.Views.Database.Model;

public class InventoryView
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string TenantName { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string UpdatedBy { get; set; }
    public bool Active { get; set; }
}
