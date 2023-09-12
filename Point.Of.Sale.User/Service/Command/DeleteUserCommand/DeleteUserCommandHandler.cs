using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.User.Service.Query.GetUserRoles;

namespace Point.Of.Sale.User.Service.Command.DeleteUserCommand;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
{
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly ISender _sender;
    private readonly UserManager<ServiceUser> _userManager;

    public DeleteUserCommandHandler(ILogger<DeleteUserCommandHandler> logger, ISender sender, UserManager<ServiceUser> userManager)
    {
        _logger = logger;
        _sender = sender;
        _userManager = userManager;
    }

    public async Task<IFluentResults<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is null)
        {
            return ResultsTo.BadRequest<bool>("User does not exist").WithMessage("Invalid argument provided.");
        }

        var userRoles = await _sender.Send(new GetUserRolesQuery(request.UserName), cancellationToken);

        if (userRoles.Value.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, userRoles.Value);
        }

        return ResultsTo.Success((await _userManager.DeleteAsync(user)).Succeeded);
    }
}
