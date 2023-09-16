using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Auth.Models;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Auth.Service.Query.GetUsers;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, List<ServiceUserResponse>>
{
    private readonly ILogger<GetUsersQueryHandler> _logger;
    private readonly ISender _sender;
    private readonly UserManager<ServiceUser> _userManager;

    public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger, UserManager<ServiceUser> userManager, ISender sender)
    {
        _logger = logger;
        _userManager = userManager;
        _sender = sender;
    }

    public async Task<IFluentResults<List<ServiceUserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _userManager.Users.ToListAsync(cancellationToken);

        if (!result.Any())
        {
            return ResultsTo.NotFound<List<ServiceUserResponse>>().WithMessage("No Users Found");
        }

        var response = result.Where(r => r.Active).Select(m => new ServiceUserResponse
        {
            FirstName = m.FirstName,
            MiddleName = m.MiddleName,
            LastName = m.LastName,
            UserName = m.UserName ?? string.Empty,
            TenantId = m.TenantId,
        }).ToList();

        return ResultsTo.Something(response);
    }
}
