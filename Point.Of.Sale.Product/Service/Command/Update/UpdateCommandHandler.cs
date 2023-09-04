using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Product.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Product.Service.Command.Update;

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
        var result = await _repository.Update(new Persistence.Models.Product
        {
            Id = request.Id,
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
