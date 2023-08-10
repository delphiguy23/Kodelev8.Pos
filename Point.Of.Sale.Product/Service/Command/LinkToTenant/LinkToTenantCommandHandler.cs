using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Product.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Product.Service.Command.LinkToTenant;

public class LinkToTenantCommandHandler : ICommandHandler<LinkToTenantCommand>
{
    private readonly IRepository _repository;

    public LinkToTenantCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(LinkToTenantCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.LinkToTenant(new Sale.Shared.Models.LinkToTenant
        {
            TenantId = request.tenantId,
            EntityId = request.entityId
        }, cancellationToken);

        if (result.IsNotFound()) return ResultsTo.NotFound().WithMessage("Product Not Found");

        return ResultsTo.Success();
    }
}
