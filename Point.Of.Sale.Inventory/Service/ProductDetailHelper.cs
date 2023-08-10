using MediatR;
using Point.Of.Sale.Category.Models;
using Point.Of.Sale.Product.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Supplier.Models;
using Point.Of.Sale.Tenant.Models;

namespace Point.Of.Sale.Inventory.Service;

public class ProductDetailHelper
{
    private readonly ISender _sender;
    private readonly int _tenandId;
    private readonly int _categoryId;
    private readonly int _productId;
    private readonly int _supplierId;
    private Task<IFluentResults<TenantResponse>> _tenant;
    private Task<IFluentResults<CategoryResponse>> _category;
    private Task<IFluentResults<ProductResponse>> _product;
    private Task<IFluentResults<SupplierResponse>> _supplier;

    public ProductDetailHelper(ISender sender)
    {
        _sender = sender;
    }

    public ProductDetailHelper(int tenandId, int categoryId, int productId, int supplierId)
    {
        _tenandId = tenandId;
        _categoryId = categoryId;
        _productId = productId;
        _supplierId = supplierId;
    }

    public async Task GetProductDetail(CancellationToken cancellationToken)
    {
        _tenant = _sender.Send(new Tenant.Service.Query.GetTenantById.GetTenantById(_tenandId), cancellationToken);
        _category = _sender.Send(new Category.Service.Query.GetById.GetById(_categoryId), cancellationToken);
        _product = _sender.Send(new Product.Service.Query.GetById.GetById(_productId), cancellationToken);
        _supplier = _sender.Send(new Supplier.Service.Query.GetById.GetById(_supplierId), cancellationToken);

        await Task.WhenAll(_tenant, _category, _product, _supplier);
    }

    public Task<IFluentResults<TenantResponse>> TenantResponse => _tenant;
    public Task<IFluentResults<ProductResponse>> ProductResponse => _product;
    public Task<IFluentResults<CategoryResponse>> CategoryResponse => _category;
    public Task<IFluentResults<SupplierResponse>> SupplierResponse => _supplier;

}
