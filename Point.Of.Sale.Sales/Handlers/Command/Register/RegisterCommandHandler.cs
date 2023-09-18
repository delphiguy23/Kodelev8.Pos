using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Sales.Repository;
using Point.Of.Sale.Shared.Enums;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Sales.Handlers.Command.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IRepository _repository;

    public RegisterCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Add(new Persistence.Models.Sale
        {
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
            LineItems = request.LineItems,
            SubTotal = request.SubTotal,
            TaxPercentage = request.TaxPercentage,
            SalesTax = request.SalesTax,
            TotalSales = request.TotalSales,
            SaleDate = request.SaleDate,
            Active = true,
            Status = SaleStatus.OrderPlaced,
        });

        return ResultsTo.Something(result);
    }
}