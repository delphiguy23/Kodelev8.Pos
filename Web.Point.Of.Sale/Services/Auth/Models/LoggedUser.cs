namespace Web.Point.Of.Sale.Services.Auth.Models;

public record LoggedUser(string UserName, string Token, string ApiToken, string RefreshToken);
