using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Supplier.Models;
using Point.Of.Sale.Supplier.Repository;

namespace Point.Of.Sale.Supplier.Service.Query.GetAll;

public sealed class GetAllQueryHandler : IQueryHandler<GetAllQuery, List<SupplierResponse>>
{
    private readonly IRepository _repository;

    public GetAllQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<SupplierResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.All(cancellationToken);

        if (result.IsNotFound()) return ResultsTo.NotFound<List<SupplierResponse>>().WithMessage("Supplier Not Found");
        if (result.IsFailure()) return ResultsTo.Failure<List<SupplierResponse>>().WithMessage(result.Messages);

        var response = result.Value.Select(r => new SupplierResponse
            {
                Id = r.Id,
                Name = r.Name,
                Address = r.Address,
                Phone = r.Phone,
                Email = r.Email,
                City = r.City,
                State = r.State,
                Country = r.Country,
                Active = r.Active,
                CreatedOn = r.CreatedOn,
                UpdatedOn = r.UpdatedOn,
                TenantId = r.TenantId
            })
            .ToList();

        return ResultsTo.Success(response);
    }
}
