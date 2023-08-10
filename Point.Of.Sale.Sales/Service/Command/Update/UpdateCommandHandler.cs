using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Sales.Models;
using Point.Of.Sale.Sales.Repository;
using Point.Of.Sale.Shared.Enums;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Sales.Service.Command.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand>
{
    private readonly IRepository _repository;

    public UpdateCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Update(new UpsertSale
        {
            Id = request.Id,
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
            LineItems = request.LineItems,
            SubTotal = request.SubTotal,
            TotalDiscounts =  request.TotalDiscounts,
            TaxPercentage = request.TaxPercentage,
            SalesTax = request.SalesTax,
            TotalSales = request.TotalSales,
            SaleDate = DateTime.UtcNow,
            Active = true,
            Status = SaleStatus.OrderPlaced
        }, cancellationToken);

        return ResultsTo.Success();
    }
}
