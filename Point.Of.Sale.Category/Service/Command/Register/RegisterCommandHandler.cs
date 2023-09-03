using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Category.Repository;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Category.Service.Command.Register;

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
        // var result = await
        _repository.Add(new Persistence.Models.Category
        {
            TenantId = request.TenantId,
            Name = request.Name,
            Description = request.Description,
            Active = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
        });

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ResultsTo.Success();
    }
}
