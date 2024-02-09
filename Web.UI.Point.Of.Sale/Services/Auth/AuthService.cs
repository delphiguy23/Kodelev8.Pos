using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json.Linq;
using Point.Of.Sale.Shared.FluentResults;
using Web.UI.Point.Of.Sale.Services.Auth.Models;

namespace Web.UI.Point.Of.Sale.Services.Auth;

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

    public async Task<IFluentResults<string>> ValidateUser(ValidateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (await _client.PostAsJsonAsync("api/auth/users/login", request, cancellationToken) is { } response)
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return ResultsTo.NotFound<string>().WithMessage($"StatusCode: {response.StatusCode}");
                    case HttpStatusCode.Unauthorized:
                        return ResultsTo.UnAuthorized<string>().WithMessage($"StatusCode: {response.StatusCode}");
                    case HttpStatusCode.OK:
                        var token = await response.Content.ReadFromJsonAsync<string>();
                        return ResultsTo.Something(token ?? string.Empty);
                    default:
                        return ResultsTo.Failure<string>().WithMessage($"StatusCode: {response.StatusCode}");
                }

            return ResultsTo.Failure<string>().WithMessage("Failed to validate user");
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<string>()
                .FromException(e)
                .WithMessage($"Failed to validate user {request.UserName}");
        }
    }

    public async Task<IFluentResults<LoggedUser>> GetUserDetails(UserExistRequest request,
        CancellationToken cancellationToken = default)
    {
        var countRetries = 0;
        var token = await _protectedSessionStorage.GetAsync<string>("token");

        if (!token.Success || string.IsNullOrWhiteSpace(token.Value))
            return ResultsTo.Failure<LoggedUser>("Token not found");

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

        return ResultsTo.Something(loggedUser);
    }

    public async Task<IFluentResults<bool>> ClearSession(CancellationToken cancellationToken = default)
    {
        try
        {
            await _protectedSessionStorage.DeleteAsync("token");
            await _protectedSessionStorage.DeleteAsync("user");
            await _protectedSessionStorage.DeleteAsync("email");
            await _protectedSessionStorage.DeleteAsync("tenant");
            await _protectedSessionStorage.DeleteAsync("isLogged");

            return ResultsTo.Something(true);
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }
}
