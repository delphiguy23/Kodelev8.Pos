using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Initializable;
using Point.Of.Sale.Persistence.Models;

namespace Point.Of.Sale.User.IdentityContext;

public interface IUsersDbContext : IInitializable
{
    DbSet<ServiceUser> ServiceUsers { get; set; }
}
