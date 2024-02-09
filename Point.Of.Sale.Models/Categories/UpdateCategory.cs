namespace Point.Of.Sale.Models.Categories;

public class UpdateCategory
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Active { get; set; }
}
