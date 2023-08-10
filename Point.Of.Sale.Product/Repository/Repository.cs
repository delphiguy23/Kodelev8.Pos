using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Product.Database.Context;
using Point.Of.Sale.Product.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Product.Repository;

public class Repository : IRepository
{
    private readonly IProductDbContext _dbContext;

    public Repository(IProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<List<Database.Model.Product>>> All(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Products.ToListAsync(cancellationToken);
        if (result is null || !result.Any()) return ResultsTo.NotFound<List<Database.Model.Product>>("Product not Found");
        return ResultsTo.Success(result);
    }

    public async Task<IFluentResults<Database.Model.Product>> GetById(int Id,  CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Products.FirstOrDefaultAsync(t => t.Id == Id, cancellationToken);

        if (result is null) return ResultsTo.NotFound<Database.Model.Product>();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Add(UpsertProduct newInventory, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Products.AddAsync(new Database.Model.Product
            {
                TenantId = newInventory.TenantId,
                SkuCode = newInventory.SkuCode,
                Name = newInventory.Name,
                Description = newInventory.Description,
                UnitPrice = newInventory.UnitPrice,
                SupplierId = newInventory.SupplierId,
                CategoryId = newInventory.CategoryId,
                Active = true,
                CreatedOn = DateTime.Now.ToUniversalTime(),
                UpdatedOn = DateTime.Now.ToUniversalTime(),
                UpdatedBy = "System",
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
        var tenant = await _dbContext.Products.FirstOrDefaultAsync(t => t.Id == linkToTenant.EntityId, cancellationToken);

        if (tenant is null) return ResultsTo.NotFound();

        tenant.TenantId = linkToTenant.TenantId;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }

    public async Task<IFluentResults<List<Database.Model.Product>>> GetByTenantId(int Id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Products.Where(t => t.TenantId == Id).ToListAsync(cancellationToken);

        if (!result.Any()) return ResultsTo.NotFound<List<Database.Model.Product>>();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Update(UpsertProduct request, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Products.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (result is null) return ResultsTo.NotFound();

        result.TenantId = request.TenantId;
        result.SkuCode = request.SkuCode;
        result.Name = request.Name;
        result.Description = request.Description;
        result.UnitPrice = request.UnitPrice;
        result.SupplierId = request.SupplierId;
        result.CategoryId = request.CategoryId;
        result.Active = request.Active;
        result.UpdatedOn = DateTime.Now.ToUniversalTime();
        result.UpdatedBy = "System";

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }
}
