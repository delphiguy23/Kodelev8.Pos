using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.Of.Sale.Category.Service.Query.GetById;
using Point.Of.Sale.Product.Models;
using Point.Of.Sale.Product.Service.Command.LinkToTenant;
using Point.Of.Sale.Product.Service.Command.Register;
using Point.Of.Sale.Product.Service.Command.Update;
using Point.Of.Sale.Product.Service.Query.GetAll;
using Point.Of.Sale.Product.Service.Query.GetByTenantId;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Tenant.Service.Query.GetTenantById;

namespace Point.Of.Sale.Product.Controller;

[ApiController]
[Route("/api/product/")]
public class ProductController : ControllerBase
{
    private readonly ISender _sender;

    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UpsertProduct request, CancellationToken cancellationToken = default)
    {
        var category = _sender.Send(new GetById(request.CategoryId), cancellationToken);
        var supplier = _sender.Send(new Supplier.Service.Query.GetById.GetById(request.SupplierId), cancellationToken);

        await Task.WhenAll(category, supplier);

        if ((await category).IsNotFoundOrBadRequest())
        {
            return (await category).ToActionResult();
        }

        if ((await supplier).IsNotFoundOrBadRequest())
        {
            return (await supplier).ToActionResult();
        }

        var result = await _sender.Send(new RegisterCommand
        {
            SkuCode = request.SkuCode,
            Name = request.Name,
            Description = request.Description,
            UnitPrice = request.UnitPrice,
            SupplierId = request.SupplierId,
            CategoryId = request.CategoryId,
            WebSite = request.WebSite,
            Image = request.Image,
            BarCodeType = request.BarCodeType,
            Barcode = request.Barcode,
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
        var result = await _sender.Send(new Service.Query.GetById.GetById(id), cancellationToken);
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
    public async Task<IActionResult> Update([FromBody] UpsertProduct request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateCommand
        {
            Id = request.Id,
            TenantId = request.TenantId,
            SkuCode = request.SkuCode,
            Name = request.Name,
            Description = request.Description,
            UnitPrice = request.UnitPrice,
            SupplierId = request.SupplierId,
            CategoryId = request.CategoryId,
            WebSite = request.WebSite,
            Barcode = request.Barcode,
            BarCodeType = request.BarCodeType,
            Image = request.Image,
            Active = request.Active,
        }, cancellationToken);

        if (result.IsFailure() || result.IsNotFoundOrBadRequest())
        {
            return result.ToActionResult();
        }

        return result.ToActionResult();
    }
}
