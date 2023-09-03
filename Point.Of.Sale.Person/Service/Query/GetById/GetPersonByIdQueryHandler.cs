using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Person.Models;
using Point.Of.Sale.Person.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Person.Service.Query.GetById;

internal sealed class GetPersonByIdQueryHandler : IQueryHandler<GetPersonById, PersonResponse>
{
    private readonly IRepository _repository;

    public GetPersonByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<PersonResponse>> Handle(GetPersonById request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetById(request.Id, cancellationToken);

        if (result.IsNotFound() || result.IsFailure())
        {
            return ResultsTo.NotFound<PersonResponse>();
        }

        var response = new PersonResponse
        {
            Id = result.Value!.Id,
            TenantId = result.Value.TenantId,
            FirstName = result.Value.FirstName,
            MiddleName = result.Value.MiddleName,
            LastName = result.Value.LastName,
            Suffix = result.Value.Suffix,
            Genmder = result.Value.Gender,
            BirthDate = result.Value.BirthDate,
            Address = result.Value.Address,
            Email = result.Value.Email,
            IsUser = result.Value.IsUser,
            UserDetails = result.Value.UserDetails,
            Active = result.Value.Active,
            CreatedOn = result.Value.CreatedOn,
            UpdatedOn = result.Value.UpdatedOn,
        };

        return ResultsTo.Success(response);
    }
}
