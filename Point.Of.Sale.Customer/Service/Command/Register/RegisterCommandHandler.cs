using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Models;
using Point.Of.Sale.Customer.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Customer.Service.Command.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IRepository _repository;

    public RegisterCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Add(new AddCustomer
        {
            Name = request.Name,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email
        }, cancellationToken);

        return ResultsTo.Success();
    }
}
