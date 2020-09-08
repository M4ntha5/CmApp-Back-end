using CmApp.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace CmApp.Contracts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<CarEntity> Cars { get; set; }
        public DbSet<CarMakesEntity> Makes { get; set; }
        public DbSet<CarModelEntity> Models { get; set; }
        public DbSet<PasswordResetEntity> PasswordResets { get; set; }
        public DbSet<RepairEntity> Repairs { get; set; }
        public DbSet<ShippingEntity> Shippings { get; set; }
        public DbSet<SummaryEntity> Summaries { get; set; }
        public DbSet<TrackingEntity> Trackings { get; set; }
        public DbSet<UserEntity> Users { get; set; }

    }
}
