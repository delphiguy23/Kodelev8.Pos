using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Sales.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

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
        var result = await _repository.Update(new Persistence.Models.Sale
        {
            Id = request.Id,
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
            LineItems = request.LineItems,
            SubTotal = request.SubTotal,
            TotalDiscounts = request.TotalDiscounts,
            TaxPercentage = request.TaxPercentage,
            SalesTax = request.SalesTax,
            TotalSales = request.TotalSales,
            SaleDate = DateTime.UtcNow,
            Active = request.Active,
            Status = request.Status,
        });

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
