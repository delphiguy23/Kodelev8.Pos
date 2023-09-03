using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Product.Repository;

public class Repository : GenericRepository<Persistence.Models.Product>, IRepository
{
    private new readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults> LinkToTenant(LinkToTenant linkToTenant, CancellationToken cancellationToken = default)
    {
        var tenant = await _dbContext.Products.FirstOrDefaultAsync(t => t.Id == linkToTenant.EntityId, cancellationToken);

        if (tenant is null)
        {
            return ResultsTo.NotFound();
        }

        tenant.TenantId = linkToTenant.TenantId;

        return ResultsTo.Success();
    }


    public async Task<IFluentResults<List<Persistence.Models.Product>>> GetByTenantId(int id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Products.Where(t => t.TenantId == id).ToListAsync(cancellationToken);

        return ResultsTo.Something(result);
    }
}
