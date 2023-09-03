using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Customer.Repository;

public class Repository : GenericRepository<Persistence.Models.Customer>, IRepository
{
    private new readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults> LinkToTenant(LinkToTenant linkToTenant, CancellationToken cancellationToken = default)
    {
        var tenant = await _dbContext.Customers.FirstOrDefaultAsync(t => t.Id == linkToTenant.EntityId, cancellationToken);

        if (tenant is null)
        {
            return ResultsTo.NotFound();
        }

        tenant.TenantId = linkToTenant.TenantId;

        return ResultsTo.Success();
    }

    public async Task<IFluentResults<List<Persistence.Models.Customer>>> GetByTenantId(int Id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Customers.Where(t => t.TenantId == Id).ToListAsync(cancellationToken);
        return ResultsTo.Something(result!);
    }
}
