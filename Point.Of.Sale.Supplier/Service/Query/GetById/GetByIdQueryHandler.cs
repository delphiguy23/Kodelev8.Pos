using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Supplier.Models;
using Point.Of.Sale.Supplier.Repository;

namespace Point.Of.Sale.Supplier.Service.Query.GetById;

internal sealed class GetByIdQueryHandler : IQueryHandler<GetById, SupplierResponse>
{
    private readonly IRepository _repository;

    public GetByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<SupplierResponse>> Handle(GetById request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetById(request.Id, cancellationToken);

        if (result.IsNotFound()) return ResultsTo.NotFound<SupplierResponse>().WithMessage("Supplier Not Found");
        if (result.IsFailure()) return ResultsTo.Failure<SupplierResponse>().WithMessage(result.Messages);

        var response = new SupplierResponse
        {
            Id = result.Value.Id,
            Name = result.Value.Name,
            Address = result.Value.Address,
            Phone = result.Value.Phone,
            Email = result.Value.Email,
            City = result.Value.City,
            State = result.Value.State,
            Country = result.Value.Country,
            Active = result.Value.Active,
            CreatedOn = result.Value.CreatedOn,
            UpdatedOn = result.Value.UpdatedOn,
            TenantId = result.Value.TenantId
        };

        return ResultsTo.Success(response);
    }
}
