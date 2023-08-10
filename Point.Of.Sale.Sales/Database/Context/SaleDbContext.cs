using Microsoft.EntityFrameworkCore;

namespace Point.Of.Sale.Sales.Database.Context;

public class SaleDbContext : DbContext, ISaleDbContext
{
    public virtual DbSet<Model.Sale> Sales { get; set; }

    public SaleDbContext()
    {
    }

    public SaleDbContext(DbContextOptions<SaleDbContext> options) : base(options)
    {

    }

    public async Task Initialize() => await Database.MigrateAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Model.Sale>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LineItems).HasColumnType("jsonb");
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
