using Microsoft.Extensions.Options;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Extensions;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.Configuration;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Handlers.Command.RegisterTenant;

internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IOptions<PosConfiguration> _configuration;
    private readonly IRepository _repository;

    public RegisterCommandHandler(IRepository repository, IOptions<PosConfiguration> configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<IFluentResults> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var tenant = new Persistence.Models.Tenant
        {
            Type = request.Type,
            Code = request.Code,
            Name = request.Name,
            Email = request.Email,
            Active = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
        };

        var parameters = new TokenBuilderParameters
        {
            Claims = tenant.CreateClaims(),
            Configuration = _configuration.Value,
            ExpiresIn = TimeSpan.FromDays(3650),
        };

        tenant.TenantApiKey = parameters.GenerateToken();

        var result = await _repository.Add(tenant, cancellationToken);

        return ResultsTo.Something(result);
    }
}