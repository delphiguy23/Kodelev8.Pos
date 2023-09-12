using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Extensions;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.Configuration;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.User.Service.Query.UserExistQuery;

namespace Point.Of.Sale.User.Service.Command.GenerateTokenCommand;

public class GenerateTokenCommandHandler : ICommandHandler<GenerateTokenCommand, string>
{
    private readonly IOptions<PosConfiguration> _configuration;
    private readonly Logger<GenerateTokenCommandHandler> _logger;
    private readonly ISender _sender;

    public GenerateTokenCommandHandler(Logger<GenerateTokenCommandHandler> logger, ISender sender, IOptions<PosConfiguration> configuration)
    {
        _logger = logger;
        _sender = sender;
        _configuration = configuration;
    }

    public async Task<IFluentResults<string>> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _sender.Send(new UserExistQuery(request.UserName, request.Email, request.TenantId), cancellationToken);

            if (user.Status == FluentResultsStatus.NotFound)
            {
                return ResultsTo.NotFound<string>().FromResults(user);
            }

            var parameters = new TokenBuilderParameters
            {
                Claims = user.Value.CreateClaims(),
                Configuration = _configuration.Value,
                ExpiresIn = TimeSpan.FromHours(1),
            };

            var token = parameters.GenerateToken();
            return ResultsTo.Something(token ?? string.Empty);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to generate token for user {0}", request.UserName);
            return ResultsTo.Failure<string>()
                .FromException(e)
                .WithMessage($"Failed to generate token for user {request.UserName}")
                .WithMessage("Failed to generate token");
        }
    }
}
