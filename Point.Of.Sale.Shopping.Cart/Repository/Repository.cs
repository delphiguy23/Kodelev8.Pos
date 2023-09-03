using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Shopping.Cart.Repository;

public class Repository : GenericRepository<ShoppingCart>, IRepository
{
    private new readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default)
    {
        var tenant = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(t => t.Id == request.EntityId, cancellationToken);

        if (tenant is null)
        {
            return ResultsTo.NotFound();
        }

        tenant.TenantId = request.TenantId;

        return ResultsTo.Success();
    }

    public async Task<IFluentResults<List<ShoppingCart>>> GetByTenantId(int request, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.ShoppingCarts.Where(t => t.TenantId == request).ToListAsync(cancellationToken);

        return ResultsTo.Something(result!);
    }
}
