using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Product.Models;
using Point.Of.Sale.Product.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Product.Handlers.Query.GetById;

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

        return result.Status switch
        {
            FluentResultsStatus.NotFound => ResultsTo.NotFound<ProductResponse>().WithMessage("Product Not Found"),
            FluentResultsStatus.BadRequest => ResultsTo.BadRequest<ProductResponse>().WithMessage("Bad Request"),
            FluentResultsStatus.Failure => ResultsTo.Failure<ProductResponse>().FromResults(result),
            _ => ResultsTo.Success(new ProductResponse
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
                TenantId = result.Value.TenantId,
                WebSite = result.Value.WebSite,
                Image = result.Value.Image,
                BarCodeType = result.Value.BarCodeType,
                Barcode = result.Value.Barcode,
            }),
        };
    }
}