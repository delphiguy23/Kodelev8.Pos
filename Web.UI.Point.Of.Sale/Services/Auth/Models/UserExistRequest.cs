namespace Web.UI.Point.Of.Sale.Services.Auth.Models;

public record UserExistRequest
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public int TenantId { get; set; }
}
