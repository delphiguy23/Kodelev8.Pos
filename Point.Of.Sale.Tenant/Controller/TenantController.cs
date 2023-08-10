using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.Of.Sale.Shared.Enums;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Models;
using Point.Of.Sale.Tenant.Service.Command.RegisterTenant;
using Point.Of.Sale.Tenant.Service.Command.Update;
using Point.Of.Sale.Tenant.Service.Query.GetAllTenants;
using Point.Of.Sale.Tenant.Service.Query.GetTenantById;

namespace Point.Of.Sale.Tenant.Controller;

[ApiController]
[Route("/api/tenant/")]
public sealed class TenantController : ControllerBase
{
    private readonly ISender _sender;

    public TenantController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UpsertTenant request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new RegisterCommand
        {
            Code = request.Code,
            Name = request.Name,
            Type = TenantType.NonSpecific,
            Active = false
        }, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetTenantById(id), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAll(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut]
    public async Task<IActionResult> Upsert([FromBody] UpsertTenant request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateCommand
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Type = TenantType.NonSpecific,
            Active = false
        }, cancellationToken);
        return result.ToActionResult();
    }
}
