using MediatR;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Inventory.Handlers.Query.GetProductDetails;
using Point.Of.Sale.Inventory.Models;
using Point.Of.Sale.Inventory.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Inventory.Handlers.Query.GetAll;

public sealed class GetAllQueryHandler : IQueryHandler<GetAllQuery, List<InventoryResponse>>
{
    private readonly IRepository _repository;
    private readonly ISender _sender;

    public GetAllQueryHandler(IRepository repository, ISender sender)
    {
        _repository = repository;
        _sender = sender;
    }

    public async Task<IFluentResults<List<InventoryResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAll(cancellationToken);

        if (result.IsNotFound())
        {
            return ResultsTo.NotFound<List<InventoryResponse>>().WithMessage("Inventory Not Found");
        }

        if (result.IsFailure())
        {
            return ResultsTo.Failure<List<InventoryResponse>>().WithMessage(result.Messages);
        }

        var response = new List<InventoryResponse>();

        foreach (var r in result.Value)
        {
            var detail = await _sender.Send(new GetProductDetailsQuery(r.TenantId, r.CategoryId, r.ProductId, r.SupplierId), cancellationToken);

            response.Add(new InventoryResponse
            {
                Id = r.Id,
                TenantId = r.TenantId,
                CategoryId = r.CategoryId,
                ProductId = r.ProductId,
                SupplierId = r.SupplierId,
                TenantName = detail?.Value?.TenantResponse?.Value?.Name ?? string.Empty,
                CategoryName = detail?.Value?.CategoryResponse?.Value?.Name ?? string.Empty,
                ProductName = detail?.Value?.ProductResponse?.Value?.Name ?? string.Empty,
                SupplierName = detail?.Value?.SupplierResponse?.Value?.Name ?? string.Empty,
                Quantity = r.Quantity,
                CreatedOn = r.CreatedOn.ToLocalTime(),
                UpdatedOn = r.UpdatedOn.ToLocalTime(),
                UpdatedBy = r.UpdatedBy,
            });
        }

        return ResultsTo.Success(response);
    }
}