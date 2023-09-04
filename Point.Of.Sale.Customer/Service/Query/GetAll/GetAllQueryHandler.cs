using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Models;
using Point.Of.Sale.Customer.Repository;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Customer.Service.Query.GetAll;

public sealed class GetAllQueryHandler : IQueryHandler<GetAllQuery, List<CustomerResponse>>
{
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllQueryHandler(IRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IFluentResults<List<CustomerResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAll(cancellationToken);

        return result.Status switch
        {
            FluentResultsStatus.NotFound => ResultsTo.NotFound<List<CustomerResponse>>().WithMessage("Customer Not Found"),
            FluentResultsStatus.BadRequest => ResultsTo.BadRequest<List<CustomerResponse>>().WithMessage("Bad Request"),
            FluentResultsStatus.Failure => ResultsTo.Failure<List<CustomerResponse>>().FromResults(result),
            _ => ResultsTo.Success(result.Value.Select(r => new CustomerResponse
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
                .ToList()),
        };
    }
}
