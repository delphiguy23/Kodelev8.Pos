using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.Of.Sale.Sales.Models;
using Point.Of.Sale.Sales.Service.Command.LinkToTenant;
using Point.Of.Sale.Sales.Service.Command.Register;
using Point.Of.Sale.Sales.Service.Command.Update;
using Point.Of.Sale.Sales.Service.Command.UpsertLineItem;
using Point.Of.Sale.Sales.Service.Query.GetAll;
using Point.Of.Sale.Sales.Service.Query.GetById;
using Point.Of.Sale.Sales.Service.Query.GetByTenantId;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Tenant.Service.Query.GetTenantById;

namespace Point.Of.Sale.Sales.Controller;

[ApiController]
[Route("/api/sale/")]
public class SaleController: ControllerBase
{
    private readonly ISender _sender;

    public SaleController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UpsertSale newSale, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new RegisterCommand
        {
            CustomerId = newSale.CustomerId,
        }, cancellationToken);

        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> All(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetById(id), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost]
    [Route("{entityId:int}/{tenantId:int}")]
    public async Task<IActionResult> LinkToTenant(int entityId, int tenantId, CancellationToken cancellationToken = default)
    {
        var tenant = await _sender.Send(new GetTenantById(tenantId), cancellationToken);

        if (tenant.IsFailure() || tenant.IsNotFoundOrBadRequest())
        {
            return tenant.ToActionResult();
        }

        var result = await _sender.Send(new LinkToTenantCommand(tenantId, entityId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    [Route("tenant/{tenantId:int}")]
    public async Task<IActionResult> GetByTenantId(int tenantId, CancellationToken cancellationToken = default)
    {
        var tenant = await _sender.Send(new GetTenantById(tenantId), cancellationToken);

        if (tenant.IsFailure() || tenant.IsNotFoundOrBadRequest())
        {
            return tenant.ToActionResult();
        }

        var result = await _sender.Send(new GetByTenantIdQuery(tenantId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpsertSale request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateCommand
        {
            Id = request.Id,
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
            Status = request.Status,
        }, cancellationToken);

        if (result.IsFailure() || result.IsNotFoundOrBadRequest())
        {
            return result.ToActionResult();
        }

        return result.ToActionResult();
    }

    [HttpPut]
    [Route("line-item/")]
    public async Task<IActionResult> UpsertLineItem([FromBody] UpsertSaleLineItem request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpsertLineItemCommand
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
        }, cancellationToken);

        if (result.IsFailure() || result.IsNotFoundOrBadRequest())
        {
            return result.ToActionResult();
        }

        return result.ToActionResult();
    }
}
