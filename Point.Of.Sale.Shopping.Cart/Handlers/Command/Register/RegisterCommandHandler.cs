using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shopping.Cart.Repository;

namespace Point.Of.Sale.Shopping.Cart.Handlers.Command.Register;

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
        var result = await _repository.Add(new ShoppingCart
        {
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
            ProductId = request.ProductId,
            ItemCount = request.ItemCount,
            Active = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
        });

        return ResultsTo.Something(result);
    }
}