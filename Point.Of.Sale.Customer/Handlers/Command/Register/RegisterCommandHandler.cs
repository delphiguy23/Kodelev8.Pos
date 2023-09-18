using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Repository;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Customer.Handlers.Command.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(IRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IFluentResults> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Add(new Persistence.Models.Customer
        {
            TenantId = request.TenantId,
            Name = request.Name,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Active = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
        }, cancellationToken);

        return ResultsTo.Something(result);
    }
}