using MediatR;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Inventory.Handlers.Query.GetProductDetails;
using Point.Of.Sale.Inventory.Models;
using Point.Of.Sale.Inventory.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Inventory.Handlers.Query.GetById;

internal sealed class GetByIdQueryHandler : IQueryHandler<GetById, InventoryResponse>
{
    private readonly IRepository _repository;
    private readonly ISender _sender;

    public GetByIdQueryHandler(IRepository repository, ISender sender)
    {
        _repository = repository;
        _sender = sender;
    }

    public async Task<IFluentResults<InventoryResponse>> Handle(GetById request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetById(request.Id, cancellationToken);

        if (result.IsNotFound())
        {
            return ResultsTo.NotFound<InventoryResponse>().WithMessage("Inventory Not Found");
        }

        if (result.IsFailure())
        {
            return ResultsTo.Failure<InventoryResponse>().WithMessage(result.Messages);
        }

        var detail = await _sender.Send(new GetProductDetailsQuery(
            result.Value!.TenantId,
            result.Value.CategoryId,
            result.Value.ProductId,
            result.Value.SupplierId), cancellationToken);

        var response = new InventoryResponse
        {
            Id = result.Value.Id,
            CategoryId = result.Value.CategoryId,
            ProductId = result.Value.ProductId,
            SupplierId = result.Value.SupplierId,
            Quantity = result.Value.Quantity,
            TenantName = detail?.Value?.TenantResponse.Value.Name ?? string.Empty,
            CategoryName = detail?.Value?.CategoryResponse.Value.Name ?? string.Empty,
            ProductName = detail?.Value?.ProductResponse.Value.Name ?? string.Empty,
            SupplierName = detail?.Value?.SupplierResponse.Value.Name ?? string.Empty,
            CreatedOn = result.Value.CreatedOn.ToLocalTime(),
            UpdatedOn = result.Value.UpdatedOn.ToLocalTime(),
            UpdatedBy = result.Value.UpdatedBy,
            TenantId = result.Value.TenantId,
        };

        return ResultsTo.Success(response);
    }
}