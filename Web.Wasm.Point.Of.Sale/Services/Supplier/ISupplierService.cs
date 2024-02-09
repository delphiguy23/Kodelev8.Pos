using Point.Of.Sale.Models.Supplier;
using Point.Of.Sale.Results.FluentResults;

namespace Web.Wasm.Point.Of.Sale.Services.Supplier;

public interface ISupplierService
{
    Task<IFluentResults<List<SupplierResponse>>> GetAll(CancellationToken cancellationToken = default);

    Task<IFluentResults> Upsert(UpsertSupplier request, bool isUpdate = true,
        CancellationToken cancellationToken = default);
}
