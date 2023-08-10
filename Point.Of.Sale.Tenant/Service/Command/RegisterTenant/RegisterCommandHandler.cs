using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.Enums;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Tenant.Models;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Service.Command.RegisterTenant;

internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IRepository _repository;

    public RegisterCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Add(new UpsertTenant
        {
            Type = TenantType.NonSpecific,
            Code = request.Code,
            Name = request.Name,
            Active = true,
        }, cancellationToken);

        if (result.IsNotFound()) return ResultsTo.NotFound<TenantResponse>().WithMessage("Tenant Not Found");

        return ResultsTo.Success();
    }
}
