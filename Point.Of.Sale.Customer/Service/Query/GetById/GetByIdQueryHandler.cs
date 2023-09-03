using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Models;
using Point.Of.Sale.Customer.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Customer.Service.Query.GetById;

internal sealed class GetByIdQueryHandler : IQueryHandler<GetById, CustomerResponse>
{
    private readonly IRepository _repository;

    public GetByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<CustomerResponse>> Handle(GetById request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetById(request.Id, cancellationToken);

        if (result.IsNotFound())
        {
            return ResultsTo.NotFound<CustomerResponse>().WithMessage("Customer Not Found");
        }

        if (result.IsFailure())
        {
            return ResultsTo.Failure<CustomerResponse>().WithMessage(result.Messages);
        }

        var response = new CustomerResponse
        {
            Id = result.Value!.Id,
            Name = result.Value.Name,
            Address = result.Value.Address,
            PhoneNumber = result.Value.PhoneNumber,
            Email = result.Value.Email,
            CreatedOn = result.Value.CreatedOn,
            UpdatedOn = result.Value.UpdatedOn,
            TenantId = result.Value.TenantId,
        };

        return ResultsTo.Success(response);
    }
}
