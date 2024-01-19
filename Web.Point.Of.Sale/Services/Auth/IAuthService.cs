using Web.Point.Of.Sale.Services.Auth.Models;

namespace Web.Point.Of.Sale.Services.Auth;

public interface IAuthService
{
    Task<HttpResponseMessage> RegisterUser(RegisterUserRequest request,
        CancellationToken cancellationToken = default);

    Task<string> ValidateUser(ValidateUserRequest request,
        CancellationToken cancellationToken = default);

    Task<LoggedUser> GetUserDetails(UserExistRequest request, string token,
        CancellationToken cancellationToken = default);
}
