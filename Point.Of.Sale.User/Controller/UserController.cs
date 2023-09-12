using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Point.Of.Sale.Shared.Configuration;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.User.Models;
using Point.Of.Sale.User.Service.Query.ValidateUserQuery;

namespace Point.Of.Sale.User.Controller;

[ApiController]
[Route("/api/user/")]
public sealed class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IOptions<PosConfiguration> _options;
    private readonly ISender _sender;

    public UserController(ISender sender, ILogger<UserController> logger, IOptions<PosConfiguration> options)
    {
        _sender = sender;
        _logger = logger;
        _options = options;
    }

    [AllowAnonymous]
    [Route("validate")]
    [HttpGet]
    public async Task<ActionResult> ValidateUser([FromBody] ValidateUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new ValidateUserQuery(request.UserName, request.Password, request.Email, request.TenantId), cancellationToken);

        return result.ToActionResult();
    }

    [AllowAnonymous]
    [Route("register")]
    [HttpPost]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new ValidateUserQuery(request.UserName, request.Password, request.Email, request.TenantId), cancellationToken);
        return result.ToActionResult();
    }
}
