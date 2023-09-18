using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Shopping.Cart.Repository;

namespace Point.Of.Sale.Shopping.Cart.Handlers.Command.Update;

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
        var result = await _repository.Update(new ShoppingCart
        {
            Id = request.Id,
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
            ProductId = request.ProductId,
            ItemCount = request.ItemCount,
            Active = request.Active,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
        }, cancellationToken);

        if (result.IsNotFound())
        {
            return ResultsTo.NotFound().WithMessage("Shopping Cart Not Found");
        }

        if (result.Value.Count == 0)
        {
            return ResultsTo.NotFound().WithMessage("Shopping Cart not updated");
        }

        return ResultsTo.Something(result.Value.Entity);
    }
}