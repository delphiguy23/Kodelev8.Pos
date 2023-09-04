using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Inventory.Repository;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Inventory.Service.Command.Update;

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
        var result = await _repository.Update(new Persistence.Models.Inventory
        {
            Id = request.Id,
            TenantId = request.TenantId,
            CategoryId = request.CategoryId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            Active = request.Active,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
        }, cancellationToken);

        if (result.IsNotFound())
        {
            return ResultsTo.NotFound().WithMessage("Inventory Not Found");
        }

        if (result.Value.Count == 0)
        {
            return ResultsTo.NotFound().WithMessage("Inventory not updated");
        }

        return ResultsTo.Something(result);
    }
}
