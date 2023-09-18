using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Sales.Models;
using Point.Of.Sale.Sales.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Sales.Handlers.Command.UpsertLineItem;

public class UpsertLineItemCommandHandler : ICommandHandler<UpsertLineItemCommand>
{
    private readonly IRepository _repository;

    public UpsertLineItemCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(UpsertLineItemCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.UpsertLineItem(new UpsertSaleLineItem
        {
            LineId = request.LineId,
            TenantId = request.TenantId,
            ProductId = request.ProductId,
            ProductName = request.ProductName,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            LineDiscount = request.LineDiscount,
            Active = true,
            LineTax = request.LineTax,
            ProductDescription = request.ProductDescription,
            LineTotal = request.LineTax + request.UnitPrice * request.Quantity - request.LineDiscount,
        }, cancellationToken);

        if (result.IsNotFound())
        {
            return ResultsTo.NotFound().WithMessage("Sales Not Found");
        }

        if (result.Value.Count == 0)
        {
            return ResultsTo.NotFound().WithMessage("Sales not updated");
        }

        return ResultsTo.Something(result.Value.Entity);
    }
}