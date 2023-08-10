using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Shopping.Cart.Models;
using Point.Of.Sale.Shopping.Cart.Repository;

namespace Point.Of.Sale.Shopping.Cart.Service.Query.GetById;

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

        if (result.IsNotFound() || result.IsFailure())
        {
            return ResultsTo.NotFound<CartResponse>();
        }

        var response = new CartResponse
        {
            Id = result.Value.Id,
            CustomerId = result.Value.CustomerId,
            ProductId = result.Value.ProductId,
            ItemCount = result.Value.ItemCount,
            Active = result.Value.Active,
            CreatedOn = result.Value.CreatedOn,
            UpdatedOn = result.Value.UpdatedOn,
            TenantId = result.Value.TenantId
        };

        return ResultsTo.Success(response);
    }
}
