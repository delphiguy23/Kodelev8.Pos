using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Point.Of.Sale.Models.Supplier;
using Point.Of.Sale.Results.FluentResults;

namespace Web.Wasm.Point.Of.Sale.Services.Categories;

public class CategoriesService:ICategoriesService
{
    private readonly HttpClient _client;

    public CategoriesService(HttpClient client)
    {
        _client = client;
    }

    public async Task<IFluentResults<List<SupplierResponse>>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            if (await _client.GetAsync("api/supplier", cancellationToken) is { } response)
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return ResultsTo.NotFound<List<SupplierResponse>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                    case HttpStatusCode.Unauthorized:
                        return ResultsTo.UnAuthorized<List<SupplierResponse>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                    case HttpStatusCode.OK:
                        var suppliers = await response.Content.ReadFromJsonAsync<List<SupplierResponse>>(cancellationToken);
                        return ResultsTo.Something(suppliers);
                    default:
                        return ResultsTo.Failure<List<SupplierResponse>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                }

            return ResultsTo.Failure<List<SupplierResponse>>().WithMessage("Failed to get suppliers.");
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<List<SupplierResponse>>()
                .FromException(e)
                .WithMessage("Failed to get suppliers.");
        }
    }

    public async Task<IFluentResults> Upsert(UpsertSupplier request, bool isUpdate = true, CancellationToken cancellationToken = default)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(request);
            HttpContent httpContent = new StringContent(jsonString);
             httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue ("application/json");

            if ((isUpdate
                    ? await _client.PutAsync("api/supplier", httpContent, cancellationToken)
                    : await _client.PostAsync("api/supplier/register", httpContent, cancellationToken)) is { } response
                )
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return ResultsTo.NotFound<List<SupplierResponse>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                    case HttpStatusCode.Unauthorized:
                        return ResultsTo.UnAuthorized<List<SupplierResponse>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                    case HttpStatusCode.OK:
                        var suppliers = await response.Content.ReadFromJsonAsync<List<SupplierResponse>>(cancellationToken);
                        return ResultsTo.Something(suppliers);
                    default:
                        return ResultsTo.Failure<List<SupplierResponse>>()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                }

            return ResultsTo.Failure<List<SupplierResponse>>().WithMessage("Failed to get suppliers.");
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<List<SupplierResponse>>()
                .FromException(e)
                .WithMessage("Failed to get suppliers.");
        }
    }
}
