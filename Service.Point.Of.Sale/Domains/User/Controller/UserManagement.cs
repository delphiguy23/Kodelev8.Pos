using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Service.Point.Of.Sale.Domains.User.Models;
using Supabase;

namespace Service.Point.Of.Sale.Domains.User.Controller;

public static class UserManagement
{
    public static void RegisterUserManagementEndpoints(this IEndpointRouteBuilder endpoints, Client client)
    {
        endpoints.MapGet("/api/user/{id}", (int id) => Results.Ok(id));

        endpoints.MapPost("/api/user/add", async (UserSignInModel signin) =>
            await client.Auth.SignUp(signin.username, signin.password));

        endpoints.MapPost("/api/user/signin", async (UserSignInModel signin) =>
            await client.Auth.SignIn(signin.username, signin.password));

        endpoints.MapPost("/api/user/signout", async () => await client.Auth.SignOut());

        endpoints.MapPost("/api/user/session", () => client.Auth.CurrentSession);
    }
}