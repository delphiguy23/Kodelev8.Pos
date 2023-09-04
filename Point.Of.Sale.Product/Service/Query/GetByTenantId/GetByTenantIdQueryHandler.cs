using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Product.Models;
using Point.Of.Sale.Product.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Product.Service.Query.GetByTenantId;

public sealed class GetByTenantIdQueryHandler : IQueryHandler<GetByTenantIdQuery, List<ProductResponse>>
{
    private readonly IRepository _repository;

    public GetByTenantIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<ProductResponse>>> Handle(GetByTenantIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByTenantId(request.id, cancellationToken);

        return result.Status switch
        {
            FluentResultsStatus.NotFound => ResultsTo.NotFound<List<ProductResponse>>().WithMessage("Product Not Found"),
            FluentResultsStatus.BadRequest => ResultsTo.BadRequest<List<ProductResponse>>().WithMessage("Bad Request"),
            FluentResultsStatus.Failure => ResultsTo.Failure<List<ProductResponse>>().FromResults(result),
            _ => ResultsTo.Success(result.Value.Select(r => new ProductResponse
                {
                    Id = r.Id,
                    SkuCode = r.SkuCode,
                    Name = r.Name,
                    Description = r.Description,
                    UnitPrice = r.UnitPrice,
                    SupplierId = r.SupplierId,
                    CategoryId = r.CategoryId,
                    Active = r.Active,
                    CreatedOn = r.CreatedOn,
                    UpdatedOn = r.UpdatedOn,
                    UpdatedBy = r.UpdatedBy,
                    TenantId = r.TenantId,
                    WebSite = r.WebSite,
                    Image = r.Image,
                    BarCodeType = r.BarCodeType,
                    Barcode = r.Barcode,
                })
                .ToList()),
        };
    }
}