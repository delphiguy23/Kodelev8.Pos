using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Category.Database.Context;
using Point.Of.Sale.Category.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Category.Repository;

public class Repository : IRepository
{
    private readonly ICategoryDbContext _dbContext;

    public Repository(ICategoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<List<Database.Model.Category>>> All(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Categories.ToListAsync(cancellationToken);

        if (!result.Any()) return ResultsTo.NotFound<List<Database.Model.Category>>();

        return ResultsTo.Success(result);
    }

    public async Task<IFluentResults<Database.Model.Category>> GetById(int Id,  CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Categories.FirstOrDefaultAsync(t => t.Id == Id, cancellationToken);

        if (result is null) return ResultsTo.NotFound<Database.Model.Category>();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Add(AddCategory newCategory, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Categories.AddAsync(new Database.Model.Category
            {
                TenantId = newCategory.TenantId,
                Name = newCategory.Name,
                Description = newCategory.Description,
                CreatedOn = DateTime.Now.ToUniversalTime(),
                UpdatedOn = DateTime.Now.ToUniversalTime(),
                UpdatedBy = "System",
                Active = true,
            }, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            return ResultsTo.Failure(e).WithMessage(e.Message);
        }

        return ResultsTo.Success();
    }

    public async Task<IFluentResults> LinkToTenant(LinkToTenant linkToTenant, CancellationToken cancellationToken = default)
    {
        var tenant = await _dbContext.Categories.FirstOrDefaultAsync(t => t.Id == linkToTenant.EntityId, cancellationToken);

        if (tenant is null) return ResultsTo.NotFound();

        tenant.TenantId = linkToTenant.TenantId;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }

    public async Task<IFluentResults<List<Database.Model.Category>>> GetByTenantId(int Id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Categories.Where(t => t.TenantId == Id).ToListAsync(cancellationToken: cancellationToken);

        if (!result.Any()) return ResultsTo.NotFound<List<Database.Model.Category>>();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Update (UpdateCategory updateCategory, CancellationToken cancellationToken = default)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(t => t.Id == updateCategory.Id, cancellationToken);

        if (category is null) return ResultsTo.NotFound();

        category.Name = updateCategory.Name;
        category.Description = updateCategory.Description;
        category.UpdatedOn = DateTime.Now.ToUniversalTime();
        category.UpdatedBy = "System";

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }
}
