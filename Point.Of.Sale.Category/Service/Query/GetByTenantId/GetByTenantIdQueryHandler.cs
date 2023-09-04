using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Category.Models;
using Point.Of.Sale.Category.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Category.Service.Query.GetByTenantId;

public sealed class GetByTenantIdQueryHandler : IQueryHandler<GetByTenantIdQuery, List<CategoryResponse>>
{
    private readonly IRepository _repository;

    public GetByTenantIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<CategoryResponse>>> Handle(GetByTenantIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByTenantId(request.id, cancellationToken);

        return result.Status switch
        {
            FluentResultsStatus.NotFound => ResultsTo.NotFound<List<CategoryResponse>>().WithMessage("Category Not Found"),
            FluentResultsStatus.BadRequest => ResultsTo.BadRequest<List<CategoryResponse>>().WithMessage("Bad Request"),
            FluentResultsStatus.Failure => ResultsTo.Failure<List<CategoryResponse>>().FromResults(result),
            _ => ResultsTo.Success(result.Value.Select(r => new CategoryResponse
                {
                    Id = r.Id,
                    Name = r.Name,
                    TenantId = r.TenantId,
                    Description = r.Description,
                    CreatedOn = r.CreatedOn,
                    UpdatedOn = r.UpdatedOn,
                })
                .ToList()),
        };
    }
}
