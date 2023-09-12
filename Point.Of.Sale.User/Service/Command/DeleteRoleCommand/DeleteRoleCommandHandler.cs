using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.User.Service.Command.DeleteRoleCommand;

public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, bool>
{
    private readonly ILogger<DeleteRoleCommandHandler> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ServiceUser> _userManager;

    public DeleteRoleCommandHandler(ILogger<DeleteRoleCommandHandler> logger, UserManager<ServiceUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IFluentResults<bool>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByNameAsync(request.RoleName);

        if (role is null)
        {
            return ResultsTo.BadRequest<bool>("Role does not exist").WithMessage("Invalid argument provided.");
        }

        var usersInRole = await _userManager.GetUsersInRoleAsync(request.RoleName);

        if (usersInRole.Any(x => x.Active))
        {
            return ResultsTo.BadRequest<bool>("Role has active users").WithMessage("Invalid argument provided.");
        }

        return ResultsTo.Success((await _roleManager.DeleteAsync(role)).Succeeded);
    }
}
