using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Persistence.Trackers;
using Point.Of.Sale.Persistence.UnitOfWork;

namespace Point.Of.Sale.Persistence.Context;

public class PosDbContext : DbContext, IPosDbContext, IUnitOfWork //, IdentityDbContext<ServiceUser>
{
    public PosDbContext()
    {
    }

    public PosDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Inventory> Inventories { get; set; }
    public virtual DbSet<Person> Persons { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Models.Sale> Sales { get; set; }
    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public virtual DbSet<Supplier> Suppliers { get; set; }
    public virtual DbSet<Tenant> Tenants { get; set; }
    public virtual DbSet<AuditLog> AuditLogs { get; set; }
    public virtual DbSet<ServiceUser> ServiceUsers { get; set; }


    public async Task Initialize()
    {
        await Database.MigrateAsync();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();

        var changedTracker = TrackerHelpers.ChangedTracker(this);

        if (changedTracker.Any())
        {
            var auditLogs = changedTracker.ToAuditLogs();
            await AuditLogs.AddRangeAsync(auditLogs, cancellationToken);
        }

        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region User Identity

        var adminId = "02174cf0–9412–4cfe-afbf-59f706d72cf6";
        var roleId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";

        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Name = "SuperAdmin",
            NormalizedName = "SUPERADMIN",
            Id = roleId,
            ConcurrencyStamp = roleId,
        });

        var appUser = new ServiceUser
        {
            Id = adminId,
            Email = "admin@kodelev8.com",
            EmailConfirmed = true,
            FirstName = "Super",
            LastName = "Admin",
            MiddleName = string.Empty,
            UserName = "SuperAdmin",
            NormalizedUserName = "SuperAdmin",
            ApiToken = "JH+C1Fnv72VIXbmM8aS8+UXJ6ci8Bgtn5R1MeOksvdWz11qmVKNvVQrSsbYivtzBkBikwz6s3ycyY4nyf34i/Q==",
            RefreshToken = "dMQa7YJBXc0rgNQeBeeJnabu+mpChoi4NAkO+1WnhqS+A+fRESDU2svYGdWPTH+1OkpzeHeVBPw8TbJ9p/LKXg==",
            TenantId = 0,
            Active = true,
        };

        //set user password
        var ph = new PasswordHasher<ServiceUser>();
        appUser.PasswordHash = ph.HashPassword(appUser, "SUPERADMIN");

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ServiceUser>(entity => { entity.HasData(appUser); });

        //set user role to admin
        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(e => new
            {
                e.RoleId, e.UserId,
            });
            entity.HasData(new IdentityUserRole<string>
            {
                RoleId = roleId,
                UserId = adminId,
            });
        });

        #endregion

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId,
            });
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId,
            });
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId,
            });
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.UserDetails).HasColumnType("jsonb");
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId, e.LastName,
            });
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId,
            });
        });

        modelBuilder.Entity<Models.Sale>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LineItems).HasColumnType("jsonb");
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId,
            });
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId,
            });
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId,
            });
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id, e.Code, e.Name,
            });
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
                {
                    e.CreatedOn, e.Id,
                })
                .IsDescending(true, false);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings during migrations
        options.UseNpgsql("User Id=postgres;Password=xqdOSyXTk69227f5;Server=db.ykoorfkswtiuzwokviis.supabase.co;Port=5432;Database=postgres");

        //
        // var builder = new ConfigurationBuilder()
        //     .SetBasePath(Directory.GetCurrentDirectory())
        //     .AddJsonFile("appsettings.json");
        // var config = builder.Build();
        // var connectionString = config.GetConnectionString("DBConnectionString");
        //
        // if (!optionsBuilder.IsConfigured)
        // {
        //     //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //     optionsBuilder.UseSqlServer(connectionString);
        // }
        //
        // var connStr = _configuration.Value.Database.BuildConnectionString() ?? string.Empty;
        //
        // switch (_configuration.Value.Database.DbProvider)
        // {
        //     case DbProvider.PostgreSql:
        //         options.UseNpgsql(_configuration.Value.Database.BuildConnectionString() ?? string.Empty);
        //         break;
        //     case DbProvider.MsSql:
        //         options.UseSqlServer(_configuration.Value.Database.BuildConnectionString() ?? string.Empty);
        //         break;
        //     case DbProvider.MySql:
        //         options.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
        //         break;
        //     case DbProvider.SqLlite:
        //         options.UseSqlite(connStr);
        //         break;
        //     default:
        //         options.UseInMemoryDatabase(connStr);
        //         break;
        // }
    }
}
