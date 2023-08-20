using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Product.Models;
using Point.Of.Sale.Product.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Product.Service.Query.GetAll;

public sealed class GetAllQueryHandler : IQueryHandler<GetAllQuery, List<ProductResponse>>
{
    private readonly IRepository _repository;

    public GetAllQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<ProductResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.All(cancellationToken);

        if (result.IsNotFound())
        {
            return ResultsTo.NotFound<List<ProductResponse>>().WithMessage("Product Not Found");
        }

        if (result.IsFailure())
        {
            return ResultsTo.Failure<List<ProductResponse>>().WithMessage(result.Messages);
        }

        var response = result.Value.Select(r => new ProductResponse
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
            .ToList();

        return ResultsTo.Success(response);
    }
}
