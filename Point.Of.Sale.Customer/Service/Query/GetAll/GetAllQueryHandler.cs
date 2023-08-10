using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using IRepository = Point.Of.Sale.Customer.Repository.IRepository;

namespace Point.Of.Sale.Customer.Service.Query.GetAll;

public sealed class GetAllQueryHandler : IQueryHandler<GetAllQuery, List<CustomerResponse>>
{
    private readonly IRepository _repository;

    public GetAllQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<CustomerResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.All(cancellationToken);

        if (result.IsNotFound()) return ResultsTo.NotFound<List<CustomerResponse>>().WithMessage("Customer Not Found");
        if (result.IsFailure()) return ResultsTo.Failure<List<CustomerResponse>>().WithMessage(result.Messages);

        var response = result.Value.Select(r => new CustomerResponse()
            {
                Id = r.Id,
                Name = r.Name,
                Address = r.Address,
                PhoneNumber = r.PhoneNumber,
                Email = r.Email,
                CreatedOn = r.CreatedOn,
                UpdatedOn = r.UpdatedOn,
                TenantId = r.TenantId,
            })
            .ToList();

        return ResultsTo.Success(response);
    }
}
