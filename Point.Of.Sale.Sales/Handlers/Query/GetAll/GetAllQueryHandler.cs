using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Sales.Models;
using Point.Of.Sale.Sales.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Sales.Handlers.Query.GetAll;

public sealed class GetAllQueryHandler : IQueryHandler<GetAllQuery, List<SaleResponse>>
{
    private readonly IRepository _repository;

    public GetAllQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<SaleResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAll(cancellationToken);

        return result.Status switch
        {
            FluentResultsStatus.NotFound => ResultsTo.NotFound<List<SaleResponse>>().WithMessage("Sale Not Found"),
            FluentResultsStatus.BadRequest => ResultsTo.BadRequest<List<SaleResponse>>().WithMessage("Bad Request"),
            FluentResultsStatus.Failure => ResultsTo.Failure<List<SaleResponse>>().FromResults(result),
            _ => ResultsTo.Success(result.Value.Select(r => new SaleResponse
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    LineItems = r.LineItems,
                    Active = r.Active,
                    SubTotal = r.SubTotal,
                    TotalDiscounts = r.TotalDiscounts,
                    TaxPercentage = r.TaxPercentage,
                    SalesTax = r.SalesTax,
                    TotalSales = r.TotalSales,
                    SaleDate = r.SaleDate,
                    Status = r.Status,
                    TenantId = r.TenantId,
                })
                .ToList()),
        };
    }
}