using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Category.Repository;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Category.Service.Command.Update;

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
        var result = await _repository.Update(new Persistence.Models.Category
        {
            Id = request.Id,
            TenantId = request.TenantId,
            Name = request.Name,
            Description = request.Description,
            Active = true,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
        }, cancellationToken);

        return ResultsTo.Something(result);
    }
}
