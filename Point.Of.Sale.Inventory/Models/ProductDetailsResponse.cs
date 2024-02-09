using Point.Of.Sale.Models.Categories;
using Point.Of.Sale.Models.Supplier;
using Point.Of.Sale.Product.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Models;

namespace Point.Of.Sale.Inventory.Models;

public record ProductDetailsResponse
{
    public IFluentResults<TenantResponse> TenantResponse { get; set; }
    public IFluentResults<ProductResponse> ProductResponse { get; set; }
    public IFluentResults<CategoryResponse> CategoryResponse { get; set; }
    public IFluentResults<SupplierResponse> SupplierResponse { get; set; }
}
