using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;
using Point.Of.Sale.Supplier.Database.Context;
using Point.Of.Sale.Supplier.Models;

namespace Point.Of.Sale.Supplier.Repository;

public class Repository : IRepository
{
    private readonly ISupplierDbContext _dbContext;

    public Repository(ISupplierDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<List<Database.Model.Supplier>>> All(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Suppliers.ToListAsync(cancellationToken);

        if (result is null || !result.Any()) return ResultsTo.NotFound<List<Database.Model.Supplier>>("No Suppliers Found");

        return ResultsTo.Success(result);
    }

    public async Task<IFluentResults<Database.Model.Supplier>> GetById(int request,  CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Suppliers.FirstOrDefaultAsync(t => t.Id == request, cancellationToken);

        if (result is null) return ResultsTo.NotFound<Database.Model.Supplier>("No Supplier Found");

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Add(UpsertSupplier request, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Suppliers.AddAsync(new Database.Model.Supplier
            {
                TenantId = request.TenantId,
                Name = request.Name,
                Address = request.Address,
                Phone = request.Phone,
                Email = request.Email,
                City = request.City,
                State = request.State,
                Country = request.Country,
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

    public async Task<IFluentResults> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default)
    {
        var tenant = await _dbContext.Suppliers.FirstOrDefaultAsync(t => t.Id == request.EntityId, cancellationToken);

        if (tenant is null) return ResultsTo.NotFound();

        tenant.TenantId = request.TenantId;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }

    public async Task<IFluentResults<List<Database.Model.Supplier>>> GetByTenantId(int request, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Suppliers.Where(t => t.TenantId == request).ToListAsync(cancellationToken);

        if (!result.Any()) return ResultsTo.NotFound<List<Database.Model.Supplier>>();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Update(UpsertSupplier request, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Suppliers.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (result is null) return ResultsTo.NotFound();

        result.TenantId = request.TenantId;
        result.Name = request.Name;
        result.Address = request.Address;
        result.City = request.City;
        result.State = request.State;
        result.Country = request.Country;
        result.Phone = request.Phone;
        result.Email = request.Email;
        result.Active = request.Active;
        result.UpdatedOn = DateTime.UtcNow;
        result.UpdatedBy = "System";

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }
}
