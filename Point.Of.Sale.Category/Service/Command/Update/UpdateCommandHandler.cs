using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Category.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using IRepository = Point.Of.Sale.Category.Repository.IRepository;

namespace Point.Of.Sale.Category.Service.Command.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand>
{
    private readonly IRepository _repository;

    public UpdateCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Update(new UpdateCategory
        {
            Id = request.Id,
            TenantId = request.TenantId,
            Name = request.Name,
            Description = request.Description,
            Active = request.Active,
        }, cancellationToken);

        if (result.IsNotFound()) return ResultsTo.NotFound().WithMessage("Category Not Found");

        return ResultsTo.Success();
    }
}
