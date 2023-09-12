using System.Text.RegularExpressions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Extensions;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.Configuration;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Service.Query.GetApiKeyById;
using Point.Of.Sale.User.Service.Query.UserExistQuery;

namespace Point.Of.Sale.User.Service.Command.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, bool>
{
    private readonly IOptions<PosConfiguration> _configuration;
    private readonly ILogger<RegisterUserCommandHandler> _logger;
    private readonly ISender _sender;
    private readonly UserManager<ServiceUser> _userManager;

    public RegisterUserCommandHandler(ILogger<RegisterUserCommandHandler> logger, IOptions<PosConfiguration> configuration, UserManager<ServiceUser> userManager, ISender sender)
    {
        _logger = logger;
        _configuration = configuration;
        _userManager = userManager;
        _sender = sender;
    }

    public async Task<IFluentResults<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (!IsCredentialsProvided(request))
        {
            return ResultsTo.BadRequest<bool>().WithMessage("Invalid Credentials Provided");
        }

        if ((await _sender.Send(new UserExistQuery(request.UserName, request.Email, request.TenantId), cancellationToken)).Status == FluentResultsStatus.Success)
        {
            return ResultsTo.BadRequest<bool>().WithMessage("User already exists");
        }

        var newUser = InitializeNewUser(request);

        var parameters = new TokenBuilderParameters
        {
            Claims = newUser.CreateClaims(),
            Configuration = _configuration.Value,
            ExpiresIn = TimeSpan.FromDays(720), // 2 years
        };

        var apiKey = await _sender.Send(new GetApiKeyByIdQuery(request.TenantId), cancellationToken);

        if (apiKey is null || apiKey.Status != FluentResultsStatus.Success)
        {
            return ResultsTo.BadRequest<bool>().WithMessage("Invalid Tenant Id");
        }

        newUser.RefreshToken = parameters.GenerateToken();
        newUser.ApiToken = apiKey.Status == FluentResultsStatus.Success ? apiKey.Value : string.Empty;

        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(m => m.Description).ToList();
            errors.ForEach(m => _logger.LogError(m));
            return ResultsTo.Failure<bool>("Failed to create user").WithMessage(errors);
        }

        return ResultsTo.Something(true);
    }

    private static ServiceUser InitializeNewUser(RegisterUserCommand request)
    {
        return new ServiceUser
        {
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            UserName = request.UserName.Trim().ToUpperInvariant(),
            NormalizedUserName = request.UserName.Trim().ToUpperInvariant(),
            Email = request.Email,
            NormalizedEmail = request.Email,
            TenantId = request.TenantId,
            Active = true,
            RefreshToken = string.Empty,
            ApiToken = string.Empty,
            EmailConfirmed = true,
            PhoneNumber = request.Phone,
            PhoneNumberConfirmed = true,
        };
    }

    private static bool IsCredentialsProvided(RegisterUserCommand request)
    {
        return !string.IsNullOrWhiteSpace(request.Email) && IsValidEmail(request.Email) && !string.IsNullOrWhiteSpace(request.UserName) && !string.IsNullOrWhiteSpace(request.Password);
    }

    private static bool IsValidEmail(string email)
    {
        var regex = new Regex(@"^[a-zA-Z0-9.a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9]+\.[a-zA-Z]+");
        var match = regex.Match(email);
        return match.Success;
    }
}
