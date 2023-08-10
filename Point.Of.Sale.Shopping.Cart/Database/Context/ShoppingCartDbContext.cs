using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Shopping.Cart.Database.Model;

namespace Point.Of.Sale.Shopping.Cart.Database.Context;

public class ShoppingCartDbContext : DbContext, IShoppingCartDbContext
{
    public DbSet<Model.ShoppingCart> ShoppingCarts { get; set; }

    public ShoppingCartDbContext()
    {
    }

    public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) : base(options)
    {

    }

    public async Task Initialize() => await Database.MigrateAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id,e.TenantId
            });
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings
        options.UseNpgsql("User Id=postgres;Password=xqdOSyXTk69227f5;Server=db.ykoorfkswtiuzwokviis.supabase.co;Port=5432;Database=postgres");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}
