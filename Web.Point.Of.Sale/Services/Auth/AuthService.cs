using Web.Point.Of.Sale.Services.Auth.Models;

namespace Web.Point.Of.Sale.Services.Auth;

public class AuthService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HttpResponseMessage> RegisterUser(RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient("pos");
        var response = await client.PostAsJsonAsync("api/auth/users/register", request, cancellationToken);
        return response;
    }

    public async Task<HttpResponseMessage> ValidateUser(ValidateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient("pos");
        var response = await client.PostAsJsonAsync("api/auth/users/login", request, cancellationToken);
        return response;
    }
}
