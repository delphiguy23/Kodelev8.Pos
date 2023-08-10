using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.Of.Sale.Category.Models;
using Point.Of.Sale.Category.Service.Command.LinkToTenant;
using Point.Of.Sale.Category.Service.Command.Register;
using Point.Of.Sale.Category.Service.Command.Update;
using Point.Of.Sale.Category.Service.Query.GetAll;
using Point.Of.Sale.Category.Service.Query.GetByTenantId;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Tenant.Service.Query.GetTenantById;

namespace Point.Of.Sale.Category.Controller;

[ApiController]
// [Route("[controller]")]
[Route("/api/category/")]
public class CategoryController: ControllerBase
{
    private readonly ISender _sender;

    public CategoryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterCategory([FromBody] AddCategory newCategory, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new RegisterCommand
        {
            TenantId = newCategory.TenantId,
            Name = newCategory.Name,
            Description = newCategory.Description,
        }, cancellationToken);

        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> All(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllCategoriesQuery(), cancellationToken);
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
    public async Task<IActionResult> GetById(int tenantId, CancellationToken cancellationToken = default)
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
    public async Task<IActionResult> Update([FromBody] UpdateCategory request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateCommand
        {
            Id = request.Id,
            TenantId = request.TenantId,
            Name = request.Name,
            Description = request.Description,
            Active = request.Active,
        }, cancellationToken);

        if (result.IsFailure() || result.IsNotFoundOrBadRequest())
        {
            return result.ToActionResult();
        }

        return result.ToActionResult();
    }

    // [HttpGet]
    // [Route("test")]
    // public  Task<IActionResult> Test(CancellationToken cancellationToken = default)
    // {
    //     var result = ResultsTo.Success<int>(50);
    //
    //     if (result.IsFailure() || result.IsNotFoundOrBadRequest())
    //     {
    //         return (ResultsTo.Something<int>(result)).ToActionResult();
    //     }
    //
    //     // var test = result
    //     //     .OnFailure( () => return result.ToActionResult())
    //     //     .OnNotFound(() => ResultsTo.NotFound("Test Not Found"))
    //     //     .OnSuccess(() => ResultsTo.Success());
    //     //
    //     // return result.ToActionResult();
    // }
}
