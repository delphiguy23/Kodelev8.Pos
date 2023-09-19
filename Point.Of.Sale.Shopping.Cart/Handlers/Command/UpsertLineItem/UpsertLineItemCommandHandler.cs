using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Shopping.Cart.Repository;

namespace Point.Of.Sale.Shopping.Cart.Handlers.Command.UpsertLineItem;

public class UpsertLineItemCommandHandler : ICommandHandler<UpsertLineItemCommand>
{
    private readonly IRepository _repository;

    public UpsertLineItemCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(UpsertLineItemCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.UpsertLineItem(new Models.UpsertLineItem
        {
            CartId = request.CartId,
            LineId = request.LineId,
            TenantId = request.TenantId,
            ProductId = request.ProductId,
            ProductName = request.ProductName,
            ProductDescription = request.ProductDescription,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            LineTotal = request.LineTotal,
        }, cancellationToken);

        if (result.IsNotFound())
        {
            return ResultsTo.NotFound().WithMessage("Shopping Cart Not Found");
        }

        if (result.Value.Count == 0)
        {
            return ResultsTo.NotFound().WithMessage("Shopping Cart line item not upserted");
        }

        return ResultsTo.Something(result.Value.Entity);
    }
}
