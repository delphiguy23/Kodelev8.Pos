using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json.Linq;
using Web.Point.Of.Sale.Services.Auth.Models;

namespace Web.Point.Of.Sale.Services.Auth;

public class AuthService : IAuthService
{
    private readonly HttpClient _client;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ProtectedSessionStorage _protectedSessionStorage;

    public AuthService(IHttpClientFactory httpClientFactory, ProtectedSessionStorage protectedSessionStorage)
    {
        _httpClientFactory = httpClientFactory;
        _protectedSessionStorage = protectedSessionStorage;
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

    public async Task<LoggedUser> GetUserDetails(UserExistRequest request,
        CancellationToken cancellationToken = default)
    {
        var token = await _protectedSessionStorage.GetAsync<string>("token");

        if (!token.Success || string.IsNullOrWhiteSpace(token.Value)) return new LoggedUser();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

        var response = await _client.PostAsJsonAsync("api/auth/users/exist", request, cancellationToken);
        var stringData = await response.Content.ReadAsStringAsync();
        var jObject = JObject.Parse(stringData);

        var loggedUser = new LoggedUser
        {
            UserName = (string)jObject.SelectToken("firstName"),
            Token = token.Value,
            ApiToken = (string)jObject.SelectToken("apiToken"),
            RefreshToken = (string)jObject.SelectToken("refreshToken")
        };

        return loggedUser;
    }
}
