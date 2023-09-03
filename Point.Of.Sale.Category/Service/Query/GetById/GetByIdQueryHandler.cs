using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Category.Models;
using Point.Of.Sale.Category.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Category.Service.Query.GetById;

public sealed class GetByIdQueryHandler : IQueryHandler<GetById, CategoryResponse>
{
    private readonly IRepository _repository;

    public GetByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<CategoryResponse>> Handle(GetById request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetById(request.id, cancellationToken);

        if (result.IsNotFound())
        {
            return ResultsTo.NotFound<CategoryResponse>().WithMessage("Category Not Found");
        }

        if (result.IsFailure())
        {
            return ResultsTo.Failure<CategoryResponse>().WithMessage(result.Messages);
        }

        var response = new CategoryResponse
        {
            Id = result.Value!.Id,
            Name = result.Value.Name,
            TenantId = result.Value.TenantId,
            Description = result.Value.Description,
            CreatedOn = result.Value.CreatedOn,
            UpdatedOn = result.Value.UpdatedOn,
        };

        return ResultsTo.Success(response);
    }
}
