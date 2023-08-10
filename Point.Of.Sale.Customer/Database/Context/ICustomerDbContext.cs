using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Initializable;

namespace Point.Of.Sale.Customer.Database.Context;

public interface ICustomerDbContext: IDbContext, IInitializable
{
    DbSet<Model.Customer> Customers { get; set; }
}
