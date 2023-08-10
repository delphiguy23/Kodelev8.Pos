using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Customer.Database.Context;
using Point.Of.Sale.Customer.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Customer.Repository;

public class Repository : IRepository
{
    private readonly ICustomerDbContext _dbContext;

    public Repository(ICustomerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<List<Database.Model.Customer>>> All(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Customers.ToListAsync(cancellationToken);
        if (result is null || !result.Any()) return ResultsTo.NotFound<List<Database.Model.Customer>>("Customer not Found");
        return ResultsTo.Success(result);
    }

    public async Task<IFluentResults<Database.Model.Customer>> GetById(int Id,  CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Customers.FirstOrDefaultAsync(t => t.Id == Id, cancellationToken);

        if (result is null) return ResultsTo.NotFound<Database.Model.Customer>();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Add(AddCustomer newCustomer, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Customers.AddAsync(new Database.Model.Customer
            {
                TenantId = newCustomer.TenantId,
                Name = newCustomer.Name,
                Address = newCustomer.Address,
                PhoneNumber = newCustomer.PhoneNumber,
                Email = newCustomer.Email,
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
        var tenant = await _dbContext.Customers.FirstOrDefaultAsync(t => t.Id == linkToTenant.EntityId, cancellationToken);

        if (tenant is null) return ResultsTo.NotFound();

        tenant.TenantId = linkToTenant.TenantId;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }

    public async Task<IFluentResults<List<Database.Model.Customer>>> GetByTenantId(int Id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Customers.Where(t => t.TenantId == Id).ToListAsync(cancellationToken: cancellationToken);

        if (!result.Any()) return ResultsTo.NotFound<List<Database.Model.Customer>>();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Update(UpdateCustomer updateCustomer, CancellationToken cancellationToken = default)
    {
        var customer = await _dbContext.Customers.FirstOrDefaultAsync(t => t.Id == updateCustomer.Id, cancellationToken);

        if (customer is null) return ResultsTo.NotFound();

        customer.TenantId = updateCustomer.TenantId;
        customer.Name = updateCustomer.Name;
        customer.Address = updateCustomer.Address;
        customer.PhoneNumber = updateCustomer.PhoneNumber;
        customer.Email = updateCustomer.Email;
        customer.Active = updateCustomer.Active;
        customer.UpdatedOn = DateTime.Now.ToUniversalTime();
        customer.UpdatedBy = "System";

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }
}
