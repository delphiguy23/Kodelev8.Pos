using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shopping.Cart.Repository;

namespace Point.Of.Sale.Shopping.Cart.Handlers.Command.LinkToTenant;

public class LinkToTenantCommandHandler : ICommandHandler<LinkToTenantCommand>
{
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public LinkToTenantCommandHandler(IRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
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