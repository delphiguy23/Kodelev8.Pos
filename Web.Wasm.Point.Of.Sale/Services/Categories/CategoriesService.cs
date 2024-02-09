using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Point.Of.Sale.Models.Categories;
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

    public async Task<IFluentResults<List<CategoryResponse>>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            if (await _client.GetAsync("api/category", cancellationToken) is { } response)
            {
                return await ProcessResponse(response, cancellationToken);
            }

            return ResultsTo.Failure<List<CategoryResponse>>().WithMessage("Failed to get categories.");
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<List<CategoryResponse>>()
                .FromException(e)
                .WithMessage("Failed to get categories.");
        }
    }

    public async Task<IFluentResults<List<CategoryResponse>>> GetByTenantId(int tenantId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await _client.GetAsync($"api/category/tenant/{tenantId}", cancellationToken) is { } response)
            {
                return await ProcessResponse(response, cancellationToken);
            }

            return ResultsTo.Failure<List<CategoryResponse>>().WithMessage("Failed to get categories.");
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<List<CategoryResponse>>()
                .FromException(e)
                .WithMessage("Failed to get categories.");
        }
    }

    public async Task<IFluentResults> Upsert(UpdateCategory request, bool isUpdate = true, CancellationToken cancellationToken = default)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(request);
            HttpContent httpContent = new StringContent(jsonString);
             httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue ("application/json");

            if ((isUpdate
                    ? await _client.PutAsync("api/category", httpContent, cancellationToken)
                    : await _client.PostAsync("api/category/register", httpContent, cancellationToken)) is { } response
                )
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return ResultsTo.NotFound()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                    case HttpStatusCode.Unauthorized:
                        return ResultsTo.UnAuthorized()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                    case HttpStatusCode.OK:
                        var suppliers = await response.Content.ReadFromJsonAsync<IFluentResults>(cancellationToken);
                        return ResultsTo.Something(suppliers);
                    default:
                        return ResultsTo.Failure()
                            .WithMessage($"StatusCode: {response.StatusCode}");
                }

            return ResultsTo.Failure().WithMessage("Failed to get categories.");
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<List<SupplierResponse>>()
                .FromException(e)
                .WithMessage("Failed to get categories.");
        }
    }

    private async Task<IFluentResults<List<CategoryResponse>>> ProcessResponse(HttpResponseMessage? response, CancellationToken cancellationToken)
    {
        if (response is null)
        {
            return ResultsTo.Failure<List<CategoryResponse>>()
                .WithMessage("Unable to get response.");
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                return ResultsTo.NotFound<List<CategoryResponse>>()
                    .WithMessage($"StatusCode: {response.StatusCode}");
            case HttpStatusCode.Unauthorized:
                return ResultsTo.UnAuthorized<List<CategoryResponse>>()
                    .WithMessage($"StatusCode: {response.StatusCode}");
            case HttpStatusCode.OK:
                var suppliers = await response.Content.ReadFromJsonAsync<List<CategoryResponse>>(cancellationToken);
                return ResultsTo.Something(suppliers);
            default:
                return ResultsTo.Failure<List<CategoryResponse>>()
                    .WithMessage($"StatusCode: {response.StatusCode}");
        }
    }
}
