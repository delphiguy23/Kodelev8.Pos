namespace Point.Of.Sale.Category.Models;

public record CategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public int TenantId { get; set; }
    public string UpdatedBy { get; set; }
}
