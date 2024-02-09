using Point.Of.Sale.Results.FluentResults;
using Web.Wasm.Point.Of.Sale.Services.Tenant.Models;

namespace Web.Wasm.Point.Of.Sale.Services.Tenant;

public interface ITenantService
{
    Task<IFluentResults<List<Web.Wasm.Point.Of.Sale.Services.Tenant.Models.Tenant>>> GetAll(CancellationToken cancellationToken = default);
    Task<IFluentResults> Upsert(UpsertTenant request, bool isUpdate, CancellationToken cancellationToken = default);
}
