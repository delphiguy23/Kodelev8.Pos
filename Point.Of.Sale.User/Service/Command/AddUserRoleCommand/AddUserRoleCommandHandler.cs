using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.User.Service.Command.AddUserRoleCommand;

public class AddUserRoleCommandHandler : ICommandHandler<AddUserRoleCommand, bool>
{
    private readonly ILogger<AddUserRoleCommandHandler> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ServiceUser> _userManager;

    public AddUserRoleCommandHandler(ILogger<AddUserRoleCommandHandler> logger, UserManager<ServiceUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IFluentResults<bool>> Handle(AddUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is null)
        {
            return ResultsTo.BadRequest<bool>("User does not exist").WithMessage("Invalid argument provided.");
        }

        var role = await _roleManager.FindByNameAsync(request.RoleName);

        if (role is null)
        {
            return ResultsTo.BadRequest<bool>("Role does not exist").WithMessage("Invalid argument provided.");
        }

        return ResultsTo.Success((await _userManager.AddToRoleAsync(user, role.Name)).Succeeded);
    }
}
