using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Sales.Database.Context;
using Point.Of.Sale.Sales.Database.Model;
using Point.Of.Sale.Sales.Models;
using Point.Of.Sale.Shared.Enums;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Sales.Repository;

public class Repository : IRepository
{
    private readonly ISaleDbContext _dbContext;

    public Repository(ISaleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<List<Sale.Sales.Database.Model.Sale>>> All(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Sales.ToListAsync(cancellationToken);
        return ResultsTo.Success(result);
    }

    public async Task<IFluentResults<Sale.Sales.Database.Model.Sale>> GetById(int Id,  CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Sales.FirstOrDefaultAsync(t => t.Id == Id, cancellationToken);

        if (result is null) ResultsTo.NotFound();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Add(UpsertSale request, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Sales.AddAsync(new Sale.Sales.Database.Model.Sale
            {
                TenantId = request.TenantId,
                CustomerId = request.CustomerId,
                LineItems = request.LineItems,
                SubTotal = request.SubTotal,
                TotalDiscounts =  request.TotalDiscounts,
                TaxPercentage = request.TaxPercentage,
                SalesTax = request.SalesTax,
                TotalSales = request.TotalSales,
                SaleDate = DateTime.UtcNow,
                Active = true,
                Status = SaleStatus.OrderPlaced
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
        var tenant = await _dbContext.Sales.FirstOrDefaultAsync(t => t.Id == linkToTenant.EntityId, cancellationToken);

        if (tenant is null) return ResultsTo.NotFound();

        tenant.TenantId = linkToTenant.TenantId;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }

    public async Task<IFluentResults<List<Sale.Sales.Database.Model.Sale>>> GetByTenantId(int Id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Sales.Where(t => t.TenantId == Id).ToListAsync(cancellationToken);

        if (!result.Any()) ResultsTo.NotFound();

        return ResultsTo.Success(result!);
    }

    public async Task<IFluentResults> Update(UpsertSale request, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Sales.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (result is null) return ResultsTo.NotFound();

        result.TenantId = request.TenantId;
        result.CustomerId = request.CustomerId;
        result.LineItems = request.LineItems;
        result.SubTotal = request.SubTotal;
        result.TotalDiscounts = request.TotalDiscounts;
        result.TaxPercentage = request.TaxPercentage;
        result.SalesTax = request.SalesTax;
        result.TotalSales = request.TotalSales;
        result.Active = request.Active;
        result.Status = request.Status;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }

    public async Task<IFluentResults> UpsertLineItem(UpsertSaleLineItem request, CancellationToken cancellationToken = default)
    {
        SaleLineItem NewLine() =>
            new()
            {
                LineId = request.LineId,
                TenantId = request.TenantId,
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice,
                LineDiscount = request.LineDiscount,
                Active = true,
                LineTax = request.LineTax,
                ProductDescription = request.ProductDescription,
                LineTotal = request.LineTotal,
            };

        var result = await _dbContext.Sales.FirstOrDefaultAsync(t => t.Id == request.SaleId, cancellationToken);

        if (result is null) return ResultsTo.NotFound();

        if (!result.LineItems.Any())
        {
            result.LineItems.Add(NewLine());
        }

        var lineItem = result.LineItems.FirstOrDefault(t => t.LineId == request.LineId);

        if (lineItem is null) result.LineItems.Add(NewLine());
        else
        {
            lineItem.TenantId = request.TenantId;
            lineItem.ProductId = request.ProductId;
            lineItem.ProductName = request.ProductName;
            lineItem.ProductDescription = request.ProductDescription;
            lineItem.Quantity = request.Quantity;
            lineItem.UnitPrice = request.UnitPrice;
            lineItem.LineDiscount = request.LineDiscount;
            lineItem.LineTax = request.LineTax;
            lineItem.LineTotal = request.LineTotal;
            lineItem.Active = request.Active;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Success();
    }
}
