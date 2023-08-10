using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Product.Models;
using Point.Of.Sale.Product.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Product.Service.Query.GetById;

internal sealed class GetByIdQueryHandler : IQueryHandler<GetById, ProductResponse>
{
    private readonly IRepository _repository;

    public GetByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<ProductResponse>> Handle(GetById request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetById(request.Id, cancellationToken);

        if (result.IsNotFound()) return ResultsTo.NotFound<ProductResponse>().WithMessage("Product Not Found");
        if (result.IsFailure()) return ResultsTo.Failure<ProductResponse>().WithMessage(result.Messages);

        var response = new ProductResponse
        {
            Id = result.Value.Id,
            SkuCode = result.Value.SkuCode,
            Name = result.Value.Name,
            Description = result.Value.Description,
            UnitPrice = result.Value.UnitPrice,
            SupplierId = result.Value.SupplierId,
            CategoryId = result.Value.CategoryId,
            Active = result.Value.Active,
            CreatedOn = result.Value.CreatedOn,
            UpdatedOn = result.Value.UpdatedOn,
            UpdatedBy = result.Value.UpdatedBy,
            TenantId = result.Value.TenantId
        };

        return ResultsTo.Success(response);
    }
}
