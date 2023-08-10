using Microsoft.EntityFrameworkCore;

namespace Point.Of.Sale.Db.Views.Database.Context;

public class ViewsDbContext : DbContext, IViewsDbContext
{
    public virtual DbSet<Model.ProductView> ProductsView { get; set; }
    public virtual DbSet<Model.InventoryView> InventoriesView { get; set; }

    public ViewsDbContext()
    {
    }

    public ViewsDbContext(DbContextOptions<ViewsDbContext> options) : base(options)
    {

    }

    public async Task Initialize() => await Database.MigrateAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Model.ProductView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("ProductsView");
            entity.HasKey(k => k.Id);
        });

        modelBuilder.Entity<Model.InventoryView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("InventoriesView");
            entity.HasKey(k => k.Id);
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
