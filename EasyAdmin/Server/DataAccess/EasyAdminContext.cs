using EasyAdmin.Shared.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EasyAdmin.Server
{
    public class EasyAdminContext : IdentityDbContext<User>
    {
        public virtual DbSet<Adapter> Adapters { get; set; }
        public virtual DbSet<Credentials> Credentials { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<Cost> Costs { get; set; }
        public virtual DbSet<OrganizationUnit> OrganizationUnits { get; set; }
        public virtual DbSet<Audit> Audits { get; set; }
        public virtual DbSet<Vm> Vms { get; set; }
        public virtual DbSet<BackendTask> BackendTasks { get; set; }

        public EasyAdminContext(DbContextOptions<EasyAdminContext> options)
            : base(options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "EasyAdmin.db" };
        //    //string connectionString = connectionStringBuilder.ToString();
        //    //SqliteConnection connection = new SqliteConnection(connectionString);
        //    //optionsBuilder.UseSqlite(connection);
        //    optionsBuilder.UseNpgsql("Host=db;Port=5432;Database=easyadmin;Username=easyadmin;Password=password");
        //}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region ProviderSeed
            modelBuilder.Entity<Provider>().HasData(new Provider { Id = (int)ProviderType.Ovirt, Name = "Ovirt", Version = "4.3", ProviderType=ProviderType.Ovirt });
            modelBuilder.Entity<Provider>().HasData(new Provider { Id = (int)ProviderType.VMware, Name = "VMware", Version = "6.7", ProviderType=ProviderType.VMware });
            #endregion

            modelBuilder.Entity<OrganizationUnit>().HasData(
                new OrganizationUnit()
                {
                    Id = 1,
                    PoolShortName = "DEF",
                    DisplayName = "Пул по умолчанию"
                }
            );

            modelBuilder.Entity<Cost>().HasIndex(i => i.DatacenterId).IsUnique();
        }
    }
}
