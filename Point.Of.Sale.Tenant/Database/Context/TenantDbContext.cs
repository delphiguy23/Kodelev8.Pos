using Microsoft.EntityFrameworkCore;

namespace Point.Of.Sale.Tenant.Database.Context;

public class TenantDbContext : DbContext, ITenantDbContext
{
    public virtual DbSet<Model.Tenant> Tenants { get; set; }

    public TenantDbContext()
    {
    }

    public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
    {

    }

    public async Task Initialize() => await Database.MigrateAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Model.Tenant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id, e.Code, e.Name
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
