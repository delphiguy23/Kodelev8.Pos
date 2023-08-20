using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Inventory.Database.Context;
using Point.Of.Sale.Inventory.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Inventory.Repository;

public class Repository : IRepository
{
    private readonly IInventoryDbContext _dbContext;

    public Repository(IInventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<List<Sale.Inventory.Database.Model.Inventory>>> All(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Inventories.ToListAsync(cancellationToken);
        if (result is null || !result.Any()) return ResultsTo.NotFound<List<Sale.Inventory.Database.Model.Inventory>>("Inventory not Found");
        return ResultsTo.Success(result);
    }

    public async Task<IFluentResults<Sale.Inventory.Database.Model.Inventory>> GetById(int Id,  CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Inventories.FirstOrDefaultAsync(t => t.Id == Id, cancellationToken);

        if (result is null) return ResultsTo.NotFound<Sale.Inventory.Database.Model.Inventory>();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Add(UpsertInventory request, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Inventories.AddAsync(new Sale.Inventory.Database.Model.Inventory
            {
                TenantId = request.TenantId,
                CategoryId = request.CategoryId,
                ProductId = request.ProductId,
                SupplierId = request.SupplierId,
                Quantity = request.Quantity,
                Active = true,
                CreatedOn = DateTime.Now.ToUniversalTime(),
                UpdatedOn = DateTime.Now.ToUniversalTime(),
                UpdatedBy = "System"
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
        var tenant = await _dbContext.Inventories.FirstOrDefaultAsync(t => t.Id == linkToTenant.EntityId, cancellationToken);

        if (tenant is null) return ResultsTo.NotFound();

        tenant.TenantId = linkToTenant.TenantId;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }

    public async Task<IFluentResults<List<Sale.Inventory.Database.Model.Inventory>>> GetByTenantId(int Id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Inventories.Where(t => t.TenantId == Id).ToListAsync(cancellationToken);

        if (!result.Any()) return ResultsTo.NotFound<List<Sale.Inventory.Database.Model.Inventory>>();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Update(UpsertInventory request, CancellationToken cancellationToken = default)
    {
        var customer = await _dbContext.Inventories.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (customer is null) return ResultsTo.NotFound();

        customer.TenantId = request.TenantId;
        customer.CategoryId = request.CategoryId;
        customer.ProductId = request.ProductId;
        customer.SupplierId = request.SupplierId;
        customer.Quantity = request.Quantity;
        customer.Active = request.Active;
        customer.UpdatedOn = DateTime.Now.ToUniversalTime();
        customer.UpdatedBy = "System";

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }
}
