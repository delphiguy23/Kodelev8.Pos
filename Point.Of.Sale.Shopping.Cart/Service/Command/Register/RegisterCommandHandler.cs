using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shopping.Cart.Models;
using Point.Of.Sale.Shopping.Cart.Repository;

namespace Point.Of.Sale.Shopping.Cart.Service.Command.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IRepository _repository;

    public RegisterCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Add(new UpsertCart
        {
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
            ProductId = request.ProductId,
            ItemCount = request.ItemCount,
        }, cancellationToken);

        return ResultsTo.Success();
    }
}
