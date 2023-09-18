using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Inventory.Repository;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Inventory.Handlers.Command.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitofwork;

    public RegisterCommandHandler(IRepository repository, IUnitOfWork unitofwork)
    {
        _repository = repository;
        _unitofwork = unitofwork;
    }

    public async Task<IFluentResults> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Add(new Persistence.Models.Inventory
        {
            TenantId = request.TenantId,
            CategoryId = request.CategoryId,
            ProductId = request.ProductId,
            SupplierId = request.SupplierId,
            Quantity = request.Quantity,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
            Active = true,
        }, cancellationToken);

        return ResultsTo.Something(result);
    }
}