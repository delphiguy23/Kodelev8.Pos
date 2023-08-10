using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shopping.Cart.Models;
using Point.Of.Sale.Shopping.Cart.Repository;

namespace Point.Of.Sale.Shopping.Cart.Service.Command.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand>
{
    private readonly IRepository _repository;

    public UpdateCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Update(new UpsertCart
        {
            Id = request.Id,
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
            ProductId = request.ProductId,
            ItemCount = request.ItemCount,
        }, cancellationToken);

        return ResultsTo.Success();
    }
}
