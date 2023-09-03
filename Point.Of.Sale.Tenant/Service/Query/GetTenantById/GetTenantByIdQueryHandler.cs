using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Models;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Service.Query.GetTenantById;

internal sealed class GetTenantByIdQueryHandler : IQueryHandler<GetTenantById, TenantResponse>
{
    private readonly IRepository _repository;

    public GetTenantByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<TenantResponse>> Handle(GetTenantById request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetById(request.Id, cancellationToken);

        return result.Status switch
        {
            FluentResultsStatus.NotFound => ResultsTo.NotFound<TenantResponse>().WithMessage("Tenant Not Found"),
            FluentResultsStatus.BadRequest => ResultsTo.BadRequest<TenantResponse>().WithMessage("Bad Request"),
            FluentResultsStatus.Failure => ResultsTo.Failure<TenantResponse>().FromResults(result),
            _ => ResultsTo.Success(new TenantResponse
            {
                Id = result.Value.Id,
                Type = result.Value.Type,
                Code = result.Value.Code,
                Name = result.Value.Name,
                Active = result.Value.Active,
                CreatedDate = result.Value.CreatedOn.ToLocalTime(),
                UpdatedBy = result.Value.UpdatedBy,
            }),
        };
    }
}
