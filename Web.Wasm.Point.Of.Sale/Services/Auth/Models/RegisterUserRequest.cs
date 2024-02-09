using Web.Wasm.Point.Of.Sale.Services.Enum;

namespace Web.Wasm.Point.Of.Sale.Services.Auth.Models;

public record RegisterUserRequest
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Suffix { get; set; }
    public Gender Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public string Address { get; set; }
    public int TenantId { get; set; }
}
