using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Database.Context;
using Point.Of.Sale.Tenant.Models;

namespace Point.Of.Sale.Tenant.Repository;

public class Repository : IRepository
{
    private readonly ITenantDbContext _dbContext;

    public Repository(ITenantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<List<Database.Model.Tenant>>> All(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Tenants.ToListAsync(cancellationToken);

        if (result is null || !result.Any()) return ResultsTo.NotFound<List<Database.Model.Tenant>>();

        return ResultsTo.Success(result);
    }

    public async Task<IFluentResults<Database.Model.Tenant>> GetById(int request,  CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Tenants.FirstOrDefaultAsync(t => t.Id == request, cancellationToken);

        if (result is null) return ResultsTo.NotFound<Database.Model.Tenant>();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Add(UpsertTenant request, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Tenants.AddAsync(new Database.Model.Tenant
            {
                Type = request.Type,
                Code = request.Code,
                Name = request.Name,
                Active = true,
                CreatedOn = DateTime.Now.ToUniversalTime(),
                UpdatedOn = DateTime.Now.ToUniversalTime(),
                CreatedBy = "System"
            }, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            return ResultsTo.Failure(e).WithMessage(e.Message);
        }

        return ResultsTo.Success();
    }

    public async Task<IFluentResults> Update(UpsertTenant request, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Tenants.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (result is null) return ResultsTo.NotFound();

        result.Type = request.Type;
        result.Code = request.Code;
        result.Name = request.Name;
        result.Active = request.Active;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }
}
