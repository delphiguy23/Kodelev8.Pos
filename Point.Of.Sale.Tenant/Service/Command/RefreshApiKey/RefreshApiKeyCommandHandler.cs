using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Extensions;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.Configuration;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Service.Command.RefreshApiKey;

public class RefreshApiKeyCommandHandler : ICommandHandler<RefreshApiKeyCommand, string>
{
    private readonly IOptions<PosConfiguration> _configuration;
    private readonly ILogger<RefreshApiKeyCommandHandler> _logger;
    private readonly IRepository _repository;
    private readonly ISender _sender;

    public RefreshApiKeyCommandHandler(ILogger<RefreshApiKeyCommandHandler> logger, IOptions<PosConfiguration> configuration, ISender sender, IRepository repository)
    {
        _logger = logger;
        _configuration = configuration;
        _sender = sender;
        _repository = repository;
    }

    public async Task<IFluentResults<string>> Handle(RefreshApiKeyCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.GetById(request.TenantId, cancellationToken) is not {Status: FluentResultsStatus.Success} tenant)
        {
            return ResultsTo.NotFound<string>().WithMessage("Tenant not found");
        }

        var parameters = new TokenBuilderParameters
        {
            Claims = tenant.Value.CreateClaims(),
            Configuration = _configuration.Value,
            ExpiresIn = TimeSpan.FromDays(3650),
        };

        if (parameters.GenerateToken() is { } apiKey && !string.IsNullOrWhiteSpace(apiKey))
        {
            var tenantPatch = new JsonPatchDocument<Persistence.Models.Tenant>().Replace(t => t.TenantApiKey, apiKey);

            return ResultsTo.Something(await _repository.Patch(tenant.Value.Id, tenantPatch, cancellationToken) is {Value.Count: > 0}
                ? apiKey
                : string.Empty);
        }

        return ResultsTo.BadRequest<string>().WithMessage("Failed to generate api key");
    }
}
