using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Tenant.Models;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Service.Query.GetAllTenants;

internal sealed class GetAllQueryHandler : IQueryHandler<GetAll, List<TenantResponse>>
{
    private readonly IRepository _repository;

    public GetAllQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<TenantResponse>>> Handle(GetAll request, CancellationToken cancellationToken)
    {
        var result = await _repository.All(cancellationToken);

        if (result.IsNotFound()) return ResultsTo.NotFound<List<TenantResponse>>().WithMessage("Tenant Not Found");
        if (result.IsFailure()) return ResultsTo.Failure<List<TenantResponse>>().WithMessage(result.Messages);

        var response = result.Value.Select(r => new TenantResponse
            {
                Id = r.Id,
                Type = r.Type,
                Code = r.Code,
                Name = r.Name,
                Active = r.Active,
                CreatedDate = r.CreatedOn.ToLocalTime(),
                CreatedBy = r.CreatedBy,
            })
            .ToList();

        return ResultsTo.Success(response);
    }

}
