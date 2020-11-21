using CmApp.Contracts.Entities;
using Microsoft.EntityFrameworkCore;


namespace CmApp.Contracts.Models
{
    public class Context : DbContext
    {
        public Context(){ }
        public Context(DbContextOptions<Context> options) : base(options) { }


        public DbSet<Car> Cars { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<Summary> Summaries { get; set; }
        public DbSet<Tracking> Trackings { get; set; }
        public DbSet<ImageUrl> Urls { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().HasOne(c => c.Shipping).WithOne(s => s.Car).HasForeignKey<Shipping>(x => x.Id);
            modelBuilder.Entity<Car>().HasOne(c => c.Summary).WithOne(s => s.Car).HasForeignKey<Summary>(x => x.Id);
            modelBuilder.Entity<Car>().HasOne(c => c.Tracking).WithOne(s => s.Car).HasForeignKey<Tracking>(x => x.Id);
        }
    }
}
