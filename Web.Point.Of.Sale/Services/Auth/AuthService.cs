using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Web.Point.Of.Sale.Services.Auth.Models;

namespace Web.Point.Of.Sale.Services.Auth;

public class AuthService : IAuthService
{
    private readonly HttpClient _client;
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _client = _httpClientFactory.CreateClient("Kodelev8-POS");
    }

    public async Task<HttpResponseMessage> RegisterUser(RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _client.PostAsJsonAsync("api/auth/users/register", request, cancellationToken);
        return response;
    }

    public async Task<string> ValidateUser(ValidateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _client.PostAsJsonAsync("api/auth/users/login", request, cancellationToken);
        var token = await response.Content.ReadFromJsonAsync<string>();

        return string.IsNullOrWhiteSpace(token) ? string.Empty : token;
    }

    public async Task<LoggedUser> GetUserDetails(UserExistRequest request, string token,
        CancellationToken cancellationToken = default)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsJsonAsync("api/auth/users/exist", request, cancellationToken);
        var stringData = await response.Content.ReadAsStringAsync();
        var jObject = JObject.Parse(stringData);

        var loggedUser = new LoggedUser(
            (string)jObject.SelectToken("firstName"),
            token,
            (string)jObject.SelectToken("apiToken"),
            (string)jObject.SelectToken("refreshToken"));

        return loggedUser;
    }
}
