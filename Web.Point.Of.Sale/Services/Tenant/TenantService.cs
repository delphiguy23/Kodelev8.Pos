namespace Web.Point.Of.Sale.Services.Tenant;

public class TenantService : ITenantService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TenantService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HttpResponseMessage> Get(CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient("Kodelev8-POS");
        var response = await client.GetAsync("api/tenant", cancellationToken);
        return response;
    }
}
