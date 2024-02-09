using System.Net;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Point.Of.Sale.Shared.FluentResults;

namespace Web.UI.Point.Of.Sale.Services.Tenant;

public class TenantService : ITenantService
{
    private readonly HttpClient _client;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ProtectedSessionStorage _protectedSessionStorage;

    public TenantService(IHttpClientFactory httpClientFactory, ProtectedSessionStorage protectedSessionStorage)
    {
        _httpClientFactory = httpClientFactory;
        _protectedSessionStorage = protectedSessionStorage;
        _client = _httpClientFactory.CreateClient("Kodelev8-POS");
    }

    public async Task<IFluentResults<List<Models.Tenant>>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            if (await _client.GetAsync("api/tenant", cancellationToken) is { } response)
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return ResultsTo.NotFound<List<Models.Tenant>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                    case HttpStatusCode.Unauthorized:
                        return ResultsTo.UnAuthorized<List<Models.Tenant>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                    case HttpStatusCode.OK:
                        var tenants = await response.Content.ReadFromJsonAsync<List<Models.Tenant>>();
                        return ResultsTo.Something(tenants);
                    default:
                        return ResultsTo.Failure<List<Models.Tenant>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                }

            return ResultsTo.Failure<List<Models.Tenant>>().WithMessage("Failed to get tenants");
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<List<Models.Tenant>>()
                .FromException(e)
                .WithMessage("Failed to get tenants");
        }
    }
}
