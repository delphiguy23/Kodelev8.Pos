using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Category.Models;
using Point.Of.Sale.Category.Repository;
using Point.Of.Sale.Shared.FluentResults;

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

        return result.Status switch
        {
            FluentResultsStatus.NotFound => ResultsTo.NotFound<CategoryResponse>().WithMessage("Category Not Found"),
            FluentResultsStatus.BadRequest => ResultsTo.BadRequest<CategoryResponse>().WithMessage("Bad Request"),
            FluentResultsStatus.Failure => ResultsTo.Failure<CategoryResponse>().FromResults(result),
            _ => ResultsTo.Success(new CategoryResponse
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                TenantId = result.Value.TenantId,
                Description = result.Value.Description,
                CreatedOn = result.Value.CreatedOn,
                UpdatedOn = result.Value.UpdatedOn,
            }),
        };
    }
}
