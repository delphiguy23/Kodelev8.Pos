using Microsoft.EntityFrameworkCore;

namespace Point.Of.Sale.Person.Database.Context;

public class PersonDbContext : DbContext, IPersonDbContext
{
    public virtual DbSet<Model.Person> Persons { get; set; }

    public PersonDbContext()
    {
    }

    public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
    {

    }

    public async Task Initialize() => await Database.MigrateAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person.Database.Model.Person>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.UserDetails).HasColumnType("jsonb");
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId, e.LastName
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
