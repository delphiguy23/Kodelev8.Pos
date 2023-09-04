using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Sales.Models;
using Point.Of.Sale.Sales.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Sales.Service.Query.GetById;

internal sealed class GetByIdQueryHandler : IQueryHandler<GetById, SaleResponse>
{
    private readonly IRepository _repository;

    public GetByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<SaleResponse>> Handle(GetById request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetById(request.Id, cancellationToken);

        return result.Status switch
        {
            FluentResultsStatus.NotFound => ResultsTo.NotFound<SaleResponse>().WithMessage("Sale Not Found"),
            FluentResultsStatus.BadRequest => ResultsTo.BadRequest<SaleResponse>().WithMessage("Bad Request"),
            FluentResultsStatus.Failure => ResultsTo.Failure<SaleResponse>().FromResults(result),
            _ => ResultsTo.Success(new SaleResponse
            {
                Id = result.Value.Id,
                CustomerId = result.Value.CustomerId,
                LineItems = result.Value.LineItems,
                Active = result.Value.Active,
                SubTotal = result.Value.SubTotal,
                TotalDiscounts = result.Value.TotalDiscounts,
                TaxPercentage = result.Value.TaxPercentage,
                SalesTax = result.Value.SalesTax,
                TotalSales = result.Value.TotalSales,
                SaleDate = result.Value.SaleDate,
                Status = result.Value.Status,
                TenantId = result.Value.TenantId,
            }),
        };
    }
}