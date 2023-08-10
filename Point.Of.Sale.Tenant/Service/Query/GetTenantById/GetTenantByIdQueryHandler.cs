using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
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

        if (result.IsNotFound()) return ResultsTo.NotFound<TenantResponse>().WithMessage("Tenant Not Found");
        if (result.IsFailure()) return ResultsTo.Failure<TenantResponse>().WithMessage(result.Messages);

        var response = new TenantResponse
        {
            Id = result.Value.Id,
            Type = result.Value.Type,
            Code = result.Value.Code,
            Name = result.Value.Name,
            Active = result.Value.Active,
            CreatedDate = result.Value.CreatedOn.ToLocalTime(),
            CreatedBy = result.Value.CreatedBy,
        };

        return ResultsTo.Success(response);
    }
}
