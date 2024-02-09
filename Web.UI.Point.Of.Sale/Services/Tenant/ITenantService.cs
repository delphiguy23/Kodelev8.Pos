using Point.Of.Sale.Shared.FluentResults;

namespace Web.UI.Point.Of.Sale.Services.Tenant;

public interface ITenantService
{
    Task<IFluentResults<List<Models.Tenant>>> GetAll(CancellationToken cancellationToken = default);
}
