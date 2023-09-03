using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Repository;

namespace Point.Of.Sale.Tenant.Repository;

public class Repository : GenericRepository<Persistence.Models.Tenant>, IRepository
{
    private new readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
