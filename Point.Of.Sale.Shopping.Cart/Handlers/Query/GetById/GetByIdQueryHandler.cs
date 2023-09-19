using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shopping.Cart.Models;
using Point.Of.Sale.Shopping.Cart.Repository;

namespace Point.Of.Sale.Shopping.Cart.Handlers.Query.GetById;

internal sealed class GetByIdQueryHandler : IQueryHandler<GetById, CartResponse>
{
    private readonly IRepository _repository;

    public GetByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<CartResponse>> Handle(GetById request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetById(request.Id, cancellationToken);

        return result.Status switch
        {
            FluentResultsStatus.NotFound => ResultsTo.NotFound<CartResponse>().WithMessage("Shopping Cart not Found"),
            FluentResultsStatus.BadRequest => ResultsTo.BadRequest<CartResponse>().WithMessage("Bad Request"),
            FluentResultsStatus.Failure => ResultsTo.Failure<CartResponse>().FromResults(result),
            _ => ResultsTo.Success(new CartResponse
            {
                Id = result.Value.Id,
                CustomerId = result.Value.CustomerId,
                ItemCount = result.Value.ItemCount,
                Active = result.Value.Active,
                CreatedOn = result.Value.CreatedOn,
                UpdatedOn = result.Value.UpdatedOn,
                TenantId = result.Value.TenantId,
            }),
        };
    }
}
