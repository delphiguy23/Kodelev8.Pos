using Point.Of.Sale.Models.Categories;
using Point.Of.Sale.Models.Supplier;
using Point.Of.Sale.Results.FluentResults;

namespace Web.Wasm.Point.Of.Sale.Services.Categories;

public interface ICategoriesService
{
    Task<IFluentResults<List<CategoryResponse>>> GetAll(CancellationToken cancellationToken = default);

    Task<IFluentResults<List<CategoryResponse>>> GetByTenantId(int tenantId,
        CancellationToken cancellationToken = default);

    Task<IFluentResults> Upsert(UpdateCategory request, bool isUpdate = true,
        CancellationToken cancellationToken = default);
}
