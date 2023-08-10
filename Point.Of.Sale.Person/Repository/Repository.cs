using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Person.Database.Context;
using Point.Of.Sale.Person.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Person.Repository;

public class Repository : IRepository
{
    private readonly IPersonDbContext _dbContext;

    public Repository(IPersonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<List<Sale.Person.Database.Model.Person>>> All(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Persons.ToListAsync(cancellationToken);
        return ResultsTo.Success(result);
    }

    public async Task<IFluentResults<Sale.Person.Database.Model.Person>> GetById(int Id,  CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Persons.FirstOrDefaultAsync(t => t.Id == Id, cancellationToken);

        if (result is null) ResultsTo.NotFound();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> AddPerson(UpsertPerson newPerson, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Persons.AddAsync(new Sale.Person.Database.Model.Person
            {
                FirstName = newPerson.FirstName,
                MiddleName = newPerson.MiddleName,
                LastName = newPerson.LastName,
                Suffix = newPerson.Suffix,
                Gender = newPerson.Gender,
                BirthDate = newPerson.BirthDate,
                Address = newPerson.Address,
                Email = newPerson.Email,
                IsUser = false,
                UserDetails = new(),
                Active = true,
                CreatedOn = DateTime.Now.ToUniversalTime(),
                UpdatedOn = DateTime.Now.ToUniversalTime(),
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
        var tenant = await _dbContext.Persons.FirstOrDefaultAsync(t => t.Id == linkToTenant.EntityId, cancellationToken);

        if (tenant is null) return ResultsTo.NotFound();

        tenant.TenantId = linkToTenant.TenantId;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }

    public async Task<IFluentResults<List<Sale.Person.Database.Model.Person>>> GetByTenantId(int Id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Persons.Where(t => t.TenantId == Id).ToListAsync(cancellationToken);

        if (!result.Any()) ResultsTo.NotFound();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Update(UpsertPerson request, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Persons.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (result is null) return ResultsTo.NotFound();

        result.TenantId = request.TenantId;
        result.FirstName = request.FirstName;
        result.MiddleName = request.MiddleName;
        result.LastName = request.LastName;
        result.Suffix = request.Suffix;
        result.Gender = request.Gender;
        result.BirthDate = request.BirthDate;
        result.Address = request.Address;
        result.Email = request.Email;
        result.IsUser = request.IsUser;
        result.UserDetails = request.UserDetails;
        result.Active = request.Active;
        result.UpdatedOn = DateTime.Now.ToUniversalTime();
        result.UpdatedBy = "System";

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }
}
