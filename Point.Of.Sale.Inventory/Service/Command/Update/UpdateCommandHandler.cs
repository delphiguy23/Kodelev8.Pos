using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Inventory.Models;
using Point.Of.Sale.Inventory.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Inventory.Service.Command.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand>
{
    private readonly IRepository _repository;

    public UpdateCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Update(new UpsertInventory
        {
            Id = request.Id,
            TenantId = request.TenantId,
            CategoryId = request.CategoryId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            Active = request.Active,
        }, cancellationToken);

        if (result.IsNotFound()) return ResultsTo.NotFound().WithMessage("Inventory Not Found");

        return ResultsTo.Success();
    }
}
