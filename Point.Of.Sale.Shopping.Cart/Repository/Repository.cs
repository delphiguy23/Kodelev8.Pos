using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Shopping.Cart.Repository;

public class Repository : GenericRepository<ShoppingCart>, IRepository
{
    private readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<CrudResult<ShoppingCart>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default)
    {
        var shoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(t => t.Id == request.EntityId, cancellationToken);

        if (shoppingCart is null)
        {
            return ResultsTo.NotFound<CrudResult<ShoppingCart>>($"No Shopping Cart found with Id {request.EntityId}.");
        }

        shoppingCart.TenantId = request.TenantId;

        return ResultsTo.Something(new CrudResult<ShoppingCart>
        {
            Count = await _dbContext.SaveChangesAsync(cancellationToken),
            Entity = shoppingCart,
        });
    }

    public async Task<IFluentResults<List<ShoppingCart>>> GetByTenantId(int request, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.ShoppingCarts.Where(t => t.TenantId == request).ToListAsync(cancellationToken);
        return ResultsTo.Something(result);
    }
}