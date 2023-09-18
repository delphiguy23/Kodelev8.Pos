using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Supplier.Models;
using Point.Of.Sale.Supplier.Repository;

namespace Point.Of.Sale.Supplier.Handlers.Query.GetByTenantId;

public sealed class GetByTenantIdQueryHandler : IQueryHandler<GetByTenantIdQuery, List<SupplierResponse>>
{
    private readonly IRepository _repository;

    public GetByTenantIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<SupplierResponse>>> Handle(GetByTenantIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByTenantId(request.id, cancellationToken);

        return result.Status switch
        {
            FluentResultsStatus.NotFound => ResultsTo.NotFound<List<SupplierResponse>>().WithMessage("Supplier Not Found"),
            FluentResultsStatus.BadRequest => ResultsTo.BadRequest<List<SupplierResponse>>().WithMessage("Bad Request"),
            FluentResultsStatus.Failure => ResultsTo.Failure<List<SupplierResponse>>().FromResults(result),
            _ => ResultsTo.Success(result.Value.Select(r => new SupplierResponse
                {
                    Id = r.Id,
                    Name = r.Name,
                    Address = r.Address,
                    Phone = r.Phone,
                    Email = r.Email,
                    City = r.City,
                    State = r.State,
                    Country = r.Country,
                    Active = r.Active,
                    CreatedOn = r.CreatedOn,
                    UpdatedOn = r.UpdatedOn,
                    TenantId = r.TenantId,
                })
                .ToList()),
        };
    }
}