using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Service.Command.Patch;

public sealed class PatchCommandHandler : ICommandHandler<PatchCommand>
{
    private readonly IRepository _repository;

    public PatchCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(PatchCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Patch(request.Id, request.Patch, cancellationToken);
        return ResultsTo.Something(result);
    }
}
