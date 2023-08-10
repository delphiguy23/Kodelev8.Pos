using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Inventory.Models;
using Point.Of.Sale.Inventory.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Inventory.Service.Command.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IRepository _repository;

    public RegisterCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Add(new UpsertInventory
        {
            TenantId = request.TenantId,
            CategoryId = request.CategoryId,
            ProductId = request.ProductId,
            SupplierId = request.SupplierId,
            Quantity = request.Quantity
        }, cancellationToken);

        return ResultsTo.Success();
    }
}
