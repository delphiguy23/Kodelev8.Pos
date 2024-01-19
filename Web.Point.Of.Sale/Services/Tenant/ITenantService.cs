namespace Web.Point.Of.Sale.Services.Tenant;

public interface ITenantService
{
    Task<HttpResponseMessage> Get(CancellationToken cancellationToken = default);
}