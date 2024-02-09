using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Point.Of.Sale.Results.FluentResults;
using Web.Wasm.Point.Of.Sale.Services.Tenant.Models;

namespace Web.Wasm.Point.Of.Sale.Services.Tenant;

public class TenantService : ITenantService
{
    private readonly HttpClient _client;
    // private readonly ProtectedSessionStorage _protectedSessionStorage;

    public TenantService(HttpClient client)
    {
        // _protectedSessionStorage = protectedSessionStorage;
        _client = client;
    }

    public async Task<IFluentResults<List<Web.Wasm.Point.Of.Sale.Services.Tenant.Models.Tenant>>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            if (await _client.GetAsync("api/tenant", cancellationToken) is { } response)
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return ResultsTo.NotFound<List<Web.Wasm.Point.Of.Sale.Services.Tenant.Models.Tenant>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                    case HttpStatusCode.Unauthorized:
                        return ResultsTo.UnAuthorized<List<Web.Wasm.Point.Of.Sale.Services.Tenant.Models.Tenant>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                    case HttpStatusCode.OK:
                        var tenants = await response.Content.ReadFromJsonAsync<List<Web.Wasm.Point.Of.Sale.Services.Tenant.Models.Tenant>>(cancellationToken);
                        return ResultsTo.Something(tenants);
                    default:
                        return ResultsTo.Failure<List<Web.Wasm.Point.Of.Sale.Services.Tenant.Models.Tenant>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                }

            return ResultsTo.Failure<List<Web.Wasm.Point.Of.Sale.Services.Tenant.Models.Tenant>>().WithMessage("Failed to get tenants");
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<List<Web.Wasm.Point.Of.Sale.Services.Tenant.Models.Tenant>>()
                .FromException(e)
                .WithMessage("Failed to get tenants");
        }
    }

    public async Task<IFluentResults> Upsert(UpsertTenant request, bool isUpdate = true, CancellationToken cancellationToken = default)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(request);
            HttpContent httpContent = new StringContent(jsonString);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue ("application/json");

            if ((isUpdate
                    ? await _client.PutAsync("api/tenant", httpContent, cancellationToken)
                    : await _client.PostAsync("api/tenant/register", httpContent, cancellationToken)) is { } response
                    )
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return ResultsTo.NotFound<List<Web.Wasm.Point.Of.Sale.Services.Tenant.Models.Tenant>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                    case HttpStatusCode.Unauthorized:
                        return ResultsTo.UnAuthorized<List<Web.Wasm.Point.Of.Sale.Services.Tenant.Models.Tenant>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                    case HttpStatusCode.OK:
                        return ResultsTo.Success();
                    default:
                        return ResultsTo.Failure<List<Web.Wasm.Point.Of.Sale.Services.Tenant.Models.Tenant>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                }

            return ResultsTo.Failure<bool>().WithMessage("Failed to upsert tenant.");
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<List<Web.Wasm.Point.Of.Sale.Services.Tenant.Models.Tenant>>()
                .FromException(e)
                .WithMessage("Failed to upsert tenant.");
        }
    }
}
