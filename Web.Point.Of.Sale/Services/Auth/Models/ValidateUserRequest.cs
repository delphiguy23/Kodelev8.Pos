namespace Web.Point.Of.Sale.Services.Auth.Models;

public record ValidateUserRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public int TenantId { get; set; }
}
