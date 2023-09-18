using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Shopping.Cart.Handlers.Command.LinkToTenant;
using Point.Of.Sale.Shopping.Cart.Handlers.Command.Register;
using Point.Of.Sale.Shopping.Cart.Handlers.Command.Update;
using Point.Of.Sale.Shopping.Cart.Handlers.Query.GetAll;
using Point.Of.Sale.Shopping.Cart.Handlers.Query.GetById;
using Point.Of.Sale.Shopping.Cart.Handlers.Query.GetByTenantId;
using Point.Of.Sale.Shopping.Cart.Models;
using Point.Of.Sale.Tenant.Handlers.Query.GetTenantById;

namespace Point.Of.Sale.Shopping.Cart.Controller;

[ApiController]
[Route("/api/shopping-cart/")]
public class ShoppingCartController : ControllerBase
{
    private readonly ISender _sender;

    public ShoppingCartController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UpsertCart request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new RegisterCommand
        {
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
            ProductId = request.ProductId,
            ItemCount = request.ItemCount,
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
    public async Task<IActionResult> Upsert([FromBody] UpsertCart request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateCommand
        {
            Id = request.Id,
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
            ProductId = request.ProductId,
            ItemCount = request.ItemCount,
        }, cancellationToken);
        return result.ToActionResult();
    }
}