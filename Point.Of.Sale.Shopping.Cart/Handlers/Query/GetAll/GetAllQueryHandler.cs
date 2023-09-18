using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shopping.Cart.Models;
using Point.Of.Sale.Shopping.Cart.Repository;

namespace Point.Of.Sale.Shopping.Cart.Handlers.Query.GetAll;

public sealed class GetAllQueryHandler : IQueryHandler<GetAllQuery, List<CartResponse>>
{
    private readonly IRepository _repository;

    public GetAllQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<CartResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAll(cancellationToken);

        return result.Status switch
        {
            FluentResultsStatus.NotFound => ResultsTo.NotFound<List<CartResponse>>().WithMessage("Cart Not Found"),
            FluentResultsStatus.BadRequest => ResultsTo.BadRequest<List<CartResponse>>().WithMessage("Bad Request"),
            FluentResultsStatus.Failure => ResultsTo.Failure<List<CartResponse>>().FromResults(result),
            _ => ResultsTo.Success(result.Value.Select(r => new CartResponse
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    ProductId = r.ProductId,
                    ItemCount = r.ItemCount,
                    Active = r.Active,
                    CreatedOn = r.CreatedOn,
                    UpdatedOn = r.UpdatedOn,
                    TenantId = r.TenantId,
                })
                .ToList()),
        };
    }
}