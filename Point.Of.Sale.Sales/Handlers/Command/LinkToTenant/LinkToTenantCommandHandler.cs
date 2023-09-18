using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Sales.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Sales.Handlers.Command.LinkToTenant;

public class LinkToTenantCommandHandler : ICommandHandler<LinkToTenantCommand>
{
    private readonly IRepository _repository;

    public LinkToTenantCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(LinkToTenantCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.LinkToTenant(new Shared.Models.LinkToTenant
        {
            TenantId = request.tenantId,
            EntityId = request.entityId,
        }, cancellationToken);

        return ResultsTo.Something(result);
    }
}