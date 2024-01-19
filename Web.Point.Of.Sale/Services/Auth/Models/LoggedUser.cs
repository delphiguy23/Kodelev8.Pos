namespace Web.Point.Of.Sale.Services.Auth.Models;

public record LoggedUser
{
    public string UserName { get; set; }
    public string Token { get; set; }
    public string ApiToken { get; set; }
    public string RefreshToken { get; set; }
}
