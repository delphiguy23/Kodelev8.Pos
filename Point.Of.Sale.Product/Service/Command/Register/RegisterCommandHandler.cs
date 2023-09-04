using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Product.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Product.Service.Command.Register;

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
        var result = await _repository.Add(new Persistence.Models.Product
        {
            TenantId = request.TenantId,
            SkuCode = request.SkuCode,
            Name = request.Name,
            Description = request.Description,
            UnitPrice = request.UnitPrice,
            SupplierId = request.SupplierId,
            CategoryId = request.CategoryId,
            WebSite = request.WebSite,
            Image = request.Image,
            BarCodeType = request.BarCodeType,
            Barcode = request.Barcode,
            Active = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
        }, cancellationToken);

        return ResultsTo.Something(result);
    }
}
