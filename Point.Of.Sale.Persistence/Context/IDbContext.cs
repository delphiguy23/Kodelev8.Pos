namespace Point.Of.Sale.Persistence.Context;

public interface IDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    // DatabaseFacade Database { get; }
}
