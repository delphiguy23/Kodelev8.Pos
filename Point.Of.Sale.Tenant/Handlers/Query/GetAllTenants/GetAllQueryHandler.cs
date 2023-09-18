using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Models;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Handlers.Query.GetAllTenants;

internal sealed class GetAllQueryHandler : IQueryHandler<GetAll, List<TenantResponse>>
{
    private readonly IRepository _repository;

    public GetAllQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<TenantResponse>>> Handle(GetAll request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAll(cancellationToken);

        return result.Status switch
        {
            FluentResultsStatus.NotFound => ResultsTo.NotFound<List<TenantResponse>>().WithMessage("Tenant Not Found"),
            FluentResultsStatus.BadRequest => ResultsTo.BadRequest<List<TenantResponse>>().WithMessage("Bad Request"),
            FluentResultsStatus.Failure => ResultsTo.Failure<List<TenantResponse>>().FromResults(result),
            _ => ResultsTo.Success(result.Value.Select(r => new TenantResponse
                {
                    Id = r.Id,
                    Type = r.Type,
                    Code = r.Code,
                    Name = r.Name,
                    Active = r.Active,
                    CreatedDate = r.CreatedOn.ToLocalTime(),
                    UpdatedBy = r.UpdatedBy,
                })
                .ToList()),
        };
    }
}