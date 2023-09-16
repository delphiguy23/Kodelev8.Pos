using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Service.Query.GetApiKeyById;

public class GetApiKeyByIdQueryHandler : IQueryHandler<GetApiKeyByIdQuery, string>
{
    private readonly IRepository _reposity;

    public GetApiKeyByIdQueryHandler(IRepository reposity)
    {
        _reposity = reposity;
    }

    public async Task<IFluentResults<string>> Handle(GetApiKeyByIdQuery request, CancellationToken cancellationToken)
    {
        var apiKey = await _reposity.GetApiKeyById(request.Id, cancellationToken);
        return ResultsTo.Something(apiKey);
    }
}
