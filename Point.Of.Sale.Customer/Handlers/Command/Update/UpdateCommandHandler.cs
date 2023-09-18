using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Repository;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Customer.Handlers.Command.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand>
{
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCommandHandler(IRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IFluentResults> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Update(new Persistence.Models.Customer
        {
            Id = request.Id,
            TenantId = request.TenantId,
            Name = request.Name,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Active = request.Active,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
        }, cancellationToken);

        return ResultsTo.Something(result);
    }
}