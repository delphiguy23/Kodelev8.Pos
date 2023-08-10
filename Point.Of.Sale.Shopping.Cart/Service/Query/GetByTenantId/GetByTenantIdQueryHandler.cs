using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Shopping.Cart.Models;
using Point.Of.Sale.Shopping.Cart.Repository;

namespace Point.Of.Sale.Shopping.Cart.Service.Query.GetByTenantId;

public sealed class GetByTenantIdQueryHandler : IQueryHandler<GetByTenantIdQuery, List<CartResponse>>
{
    private readonly IRepository _repository;

    public GetByTenantIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<CartResponse>>> Handle(GetByTenantIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByTenantId(request.id, cancellationToken);

        if (result.IsNotFound() || result.IsFailure())
        {
            return ResultsTo.Failure<List<CartResponse>>("Cart Not Found");
        }

        var response = result.Value.Select(r => new CartResponse
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                ProductId = r.ProductId,
                ItemCount = r.ItemCount,
                Active = r.Active,
                CreatedOn = r.CreatedOn,
                UpdatedOn = r.UpdatedOn,
                TenantId = r.TenantId
            })
            .ToList();

        return ResultsTo.Success(response);
    }
}
