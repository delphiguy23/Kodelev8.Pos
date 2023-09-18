using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Supplier.Repository;

namespace Point.Of.Sale.Supplier.Handlers.Command.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IRepository _repository;

    public RegisterCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Add(new Persistence.Models.Supplier
        {
            TenantId = request.TenantId,
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email,
            City = request.City,
            State = request.State,
            Country = request.Country,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            Active = true,
            UpdatedBy = "User",
        }, cancellationToken);

        return ResultsTo.Something(result);
    }
}