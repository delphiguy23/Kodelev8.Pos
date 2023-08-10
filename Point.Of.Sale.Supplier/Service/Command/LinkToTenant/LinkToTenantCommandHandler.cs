using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Supplier.Repository;

namespace Point.Of.Sale.Supplier.Service.Command.LinkToTenant;

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
            EntityId = request.entityId
        }, cancellationToken);

        if (result.IsNotFound()) return ResultsTo.NotFound().WithMessage("Supplier Not Found");

        return ResultsTo.Success();
    }
}
