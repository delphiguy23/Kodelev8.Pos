using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.User.Service.Query.ValidateUser;

public class ValidateUserQueryHandler : IQueryHandler<ValidateUserQuery, bool>
{
    private readonly ILogger<ValidateUserQueryHandler> _logger;
    private readonly UserManager<ServiceUser> _userManager;

    public ValidateUserQueryHandler(ILogger<ValidateUserQueryHandler> logger, UserManager<ServiceUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IFluentResults<bool>> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
    {
        var result = await _userManager.FindByNameAsync(request.UserName);

        if (result is null || result.Active == false || result.TenantId != request.TenantId)
        {
            return ResultsTo.NotFound<bool>().WithMessage("User does not exist");
        }

        return ResultsTo.Something(await _userManager.CheckPasswordAsync(result, request.Password));
    }
}
