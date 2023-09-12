using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.User.Service.Command.AddRoleCommand;

public class AddRoleCommandHandler : ICommandHandler<AddRoleCommand, bool>
{
    private readonly ILogger<AddRoleCommandHandler> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ISender _sender;

    public AddRoleCommandHandler(ILogger<AddRoleCommandHandler> logger, ISender sender, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _sender = sender;
        _roleManager = roleManager;
    }

    public async Task<IFluentResults<bool>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _roleManager.FindByNameAsync(request.Role);

        if (result is not null)
        {
            return ResultsTo.BadRequest<bool>("Role already exists").WithMessage("Invalid argument provided.");
        }

        return ResultsTo.Success((await _roleManager.CreateAsync(new IdentityRole {Name = request.Role})).Succeeded);
    }
}
