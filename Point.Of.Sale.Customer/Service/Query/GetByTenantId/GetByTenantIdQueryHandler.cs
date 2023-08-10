using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Models;
using Point.Of.Sale.Customer.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Customer.Service.Query.GetByTenantId;

public sealed class GetByTenantIdQueryHandler : IQueryHandler<GetByTenantIdQuery, List<CustomerResponse>>
{
    private readonly IRepository _repository;

    public GetByTenantIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<CustomerResponse>>> Handle(GetByTenantIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByTenantId(request.id, cancellationToken);

        if (result.IsNotFound()) return ResultsTo.NotFound<List<CustomerResponse>>().WithMessage("Customer Not Found");
        if (result.IsFailure()) return ResultsTo.Failure<List<CustomerResponse>>().WithMessage(result.Messages);

        var response = result.Value.Select(r => new CustomerResponse
            {
                    Id = r.Id,
                    Name = r.Name,
                    Address = r.Address,
                    PhoneNumber = r.PhoneNumber,
                    Email = r.Email,
                    CreatedOn = r.CreatedOn,
                    UpdatedOn = r.UpdatedOn,
            })
            .ToList();

        return ResultsTo.Success(response);
    }
}
