using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Models;
using Point.Of.Sale.Customer.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Customer.Service.Command.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand>
{
    private readonly IRepository _repository;

    public UpdateCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Update(new UpdateCustomer
        {
            Id = request.Id,
            TenantId = request.TenantId,
            Name = request.Name,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Active = request.Active,
        }, cancellationToken);

        if (result.IsNotFound()) return ResultsTo.NotFound().WithMessage("Customer Not Found");

        return ResultsTo.Success();
    }
}
