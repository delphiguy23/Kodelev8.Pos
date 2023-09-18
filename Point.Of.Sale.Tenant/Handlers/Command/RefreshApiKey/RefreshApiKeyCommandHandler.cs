using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Extensions;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.Configuration;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Handlers.Command.RefreshApiKey;

public class RefreshApiKeyCommandHandler : ICommandHandler<RefreshApiKeyCommand, string>
{
    private readonly IOptions<PosConfiguration> _configuration;
    private readonly ILogger<RefreshApiKeyCommandHandler> _logging;
    private readonly IRepository _repository;

    public RefreshApiKeyCommandHandler(ILogger<RefreshApiKeyCommandHandler> logging, IOptions<PosConfiguration> configuration, IRepository repository)
    {
        _logging = logging;
        _configuration = configuration;
        _repository = repository;
    }

    public async Task<IFluentResults<string>> Handle(RefreshApiKeyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await _repository.GetById(request.TenantId, cancellationToken) is not {Status: FluentResultsStatus.Success} tenant)
            {
                return ResultsTo.NotFound<string>().WithMessage("Tenant not found");
            }

            var tenantApiKey = request.ApiKey.Trim();

            if ((tenant.Value.TenantApiKey?.Trim() ?? string.Empty) != (!string.IsNullOrWhiteSpace(tenantApiKey) ? tenantApiKey : string.Empty))
            {
                return ResultsTo.BadRequest<string>().WithMessage("Api Key mismatched");
            }

            var parameters = new TokenBuilderParameters
            {
                Claims = tenant.Value.CreateClaims(),
                Configuration = _configuration.Value,
                ExpiresIn = TimeSpan.FromDays(3650),
            };

            if (parameters.GenerateToken() is { } apiKey && !string.IsNullOrWhiteSpace(apiKey))
            {
                var tenantApiKeyPatch = new JsonPatchDocument<Persistence.Models.Tenant>().Replace(t => t.TenantApiKey, apiKey);

                return ResultsTo.Something(await _repository.Patch(tenant.Value.Id, tenantApiKeyPatch, cancellationToken) is {Value.Count: > 0}
                    ? apiKey
                    : string.Empty);
            }

            return ResultsTo.BadRequest<string>().WithMessage("Failed to generate api key");
        }
        catch (Exception e)
        {
            _logging.LogError(e, "Failed to generate api key");
            return ResultsTo.Failure<string>().FromException(e).WithMessage("Failed to generate api key");
        }
    }
}
