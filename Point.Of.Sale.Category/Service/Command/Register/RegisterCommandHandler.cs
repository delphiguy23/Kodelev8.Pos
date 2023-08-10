using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Category.Models;
using Point.Of.Sale.Shared.FluentResults;
using IRepository = Point.Of.Sale.Category.Repository.IRepository;

namespace Point.Of.Sale.Category.Service.Command.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IRepository _repository;

    public RegisterCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Add(new AddCategory
        {
            TenantId = request.TenantId,
            Name = request.Name,
            Description = request.Description,
        }, cancellationToken);

        return ResultsTo.Success();
    }
}
