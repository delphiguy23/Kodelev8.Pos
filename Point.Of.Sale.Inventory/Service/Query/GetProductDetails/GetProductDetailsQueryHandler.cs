using MediatR;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Category.Models;
using Point.Of.Sale.Inventory.Models;
using Point.Of.Sale.Product.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Supplier.Models;
using Point.Of.Sale.Tenant.Models;

namespace Point.Of.Sale.Inventory.Service.Query.GetProductDetails;

public sealed class GetProductDetailsQueryHandler : IQueryHandler<GetProductDetailsQuery, ProductDetailsResponse>
{
    private readonly ISender _sender;
    private Task<IFluentResults<TenantResponse>> _tenant;
    private Task<IFluentResults<CategoryResponse>> _category;
    private Task<IFluentResults<ProductResponse>> _product;
    private Task<IFluentResults<SupplierResponse>> _supplier;

    public GetProductDetailsQueryHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IFluentResults<ProductDetailsResponse>> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
    {
        _tenant = _sender.Send(new Tenant.Service.Query.GetTenantById.GetTenantById(request.tenandId), cancellationToken);
        _category = _sender.Send(new Category.Service.Query.GetById.GetById(request.categoryId), cancellationToken);
        _product = _sender.Send(new Product.Service.Query.GetById.GetById(request.productId), cancellationToken);
        _supplier = _sender.Send(new Supplier.Service.Query.GetById.GetById(request.supplierId), cancellationToken);

        await Task.WhenAll(_tenant, _category, _product, _supplier);

        var result = new ProductDetailsResponse
        {
            TenantResponse = await _tenant,
            ProductResponse = await _product,
            CategoryResponse = await _category,
            SupplierResponse = await _supplier,
        };

        return ResultsTo.Success(result);
    }
}
