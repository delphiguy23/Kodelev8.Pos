using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.User.Service.Query.ValidateUserQuery;

public class ValidateUserQueryHandler : IQueryHandler<ValidateUserQuery, bool>
{
    private readonly ILogger<ValidateUserQueryHandler> _logger;
    private readonly ISender _sender;
    private readonly UserManager<ServiceUser> _userManager;

    public ValidateUserQueryHandler(ILogger<ValidateUserQueryHandler> logger, UserManager<ServiceUser> userManager, ISender sender)
    {
        _logger = logger;
        _userManager = userManager;
        _sender = sender;
    }

    public async Task<IFluentResults<bool>> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _sender.Send(new UserExistQuery.UserExistQuery(request.UserName, request.Email, request.TenantId), cancellationToken);
        return user.Status == FluentResultsStatus.NotFound
            ? ResultsTo.NotFound<bool>().WithMessage("User not found.")
            : ResultsTo.Something(await _userManager.CheckPasswordAsync(user.Value, request.Password));
    }
}
