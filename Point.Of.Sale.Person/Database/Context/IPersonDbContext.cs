using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Initializable;

namespace Point.Of.Sale.Person.Database.Context;

public interface IPersonDbContext: IDbContext, IInitializable
{
    DbSet<Model.Person> Persons { get; set; }
}
