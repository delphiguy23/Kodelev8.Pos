using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Supplier.Models;
using Point.Of.Sale.Supplier.Service.Command.LinkToTenant;
using Point.Of.Sale.Supplier.Service.Command.Register;
using Point.Of.Sale.Supplier.Service.Command.Update;
using Point.Of.Sale.Supplier.Service.Query.GetAll;
using Point.Of.Sale.Supplier.Service.Query.GetById;
using Point.Of.Sale.Supplier.Service.Query.GetByTenantId;
using Point.Of.Sale.Tenant.Service.Query.GetTenantById;

namespace Point.Of.Sale.Supplier.Controller;

[ApiController]
[Route("/api/supplier/")]
public class SupplierController: ControllerBase
{
    private readonly ISender _sender;

    public SupplierController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UpsertSupplier request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new RegisterCommand
        {
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email,
            City = request.City,
            State = request.State,
            Country = request.Country,
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
    public async Task<IActionResult> Upsert([FromBody] UpsertSupplier request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateCommand
        {
            Id = request.Id,
            TenantId = request.TenantId,
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email,
            City = request.City,
            State = request.State,
            Country = request.Country,
        }, cancellationToken);
        return result.ToActionResult();
    }
}