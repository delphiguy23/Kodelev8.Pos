using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;
using Point.Of.Sale.Shopping.Cart.Database.Context;
using Point.Of.Sale.Shopping.Cart.Models;

namespace Point.Of.Sale.Shopping.Cart.Repository;

public class Repository : IRepository
{
    private readonly IShoppingCartDbContext _dbContext;

    public Repository(IShoppingCartDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<List<Shopping.Cart.Database.Model.ShoppingCart>>> All(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.ShoppingCarts.ToListAsync(cancellationToken);
        return ResultsTo.Success(result);
    }

    public async Task<IFluentResults<Shopping.Cart.Database.Model.ShoppingCart>> GetById(int request,  CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(t => t.Id == request, cancellationToken);

        if (result is null) ResultsTo.NotFound();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Add(UpsertCart request, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.ShoppingCarts.AddAsync(new Shopping.Cart.Database.Model.ShoppingCart
            {
                TenantId = request.TenantId,
                CustomerId = request.CustomerId,
                ProductId = request.ProductId,
                ItemCount = request.ItemCount,
                Active = true,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            }, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            return ResultsTo.Failure(e).WithMessage(e.Message);
        }

        return ResultsTo.Success();
    }

    public async Task<IFluentResults> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default)
    {
        var tenant = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(t => t.Id == request.EntityId, cancellationToken);

        if (tenant is null) return ResultsTo.NotFound();

        tenant.TenantId = request.TenantId;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }

    public async Task<IFluentResults<List<Shopping.Cart.Database.Model.ShoppingCart>>> GetByTenantId(int request, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.ShoppingCarts.Where(t => t.TenantId == request).ToListAsync(cancellationToken);

        if (!result.Any()) ResultsTo.NotFound();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Update(UpsertCart request, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (result is null) return ResultsTo.NotFound();

        result.TenantId = request.TenantId;
        result.CustomerId = request.CustomerId;
        result.ProductId = request.ProductId;
        result.ItemCount = request.ItemCount;
        result.Active = request.Active;
        result.UpdatedOn = DateTime.UtcNow;
        result.UpdatedBy = "System";

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }
}
