using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Tenant.Models;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Service.Command.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand>
{
    private readonly IRepository _repository;

    public UpdateCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Update(new UpsertTenant
        {
            Id = request.Id,
            Type = request.Type,
            Code = request.Code,
            Name = request.Name,
            Active = true,
        }, cancellationToken);

        if (result.IsNotFound()) return ResultsTo.NotFound().WithMessage("Tenant Not Found");

        return ResultsTo.Success();
    }
}
