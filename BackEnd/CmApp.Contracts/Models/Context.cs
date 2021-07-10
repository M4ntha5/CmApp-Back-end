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
        public DbSet<Tracking> Trackings { get; set; }
        public DbSet<TrackingImage> TrackingImages { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* modelBuilder.Entity<Car>(b =>
             {
                 b.HasOne(c => c.Shipping).WithOne(s => s.Car).HasForeignKey<Shipping>(x => x.Id);
                 b.HasOne(c => c.Summary).WithOne(s => s.Car).HasForeignKey<Summary>(x => x.Id);
                 b.HasOne(c => c.Tracking).WithOne(s => s.Car).HasForeignKey<Tracking>(x => x.Id);
                 b.HasOne(c => c.User).WithMany(s => s.Cars).HasForeignKey(x => x.UserId);
                 b.HasMany(c => c.Equipment).WithOne(s => s.Car).HasForeignKey(x => x.CarId);
                 b.HasMany(c => c.Repairs).WithOne(s => s.Car).HasForeignKey(x => x.CarId);
                 b.HasMany(c => c.Images).WithOne(s => s.Car).HasForeignKey(x => x.CarId);
             });*/
            /*modelBuilder.Entity<Make>(b =>
            {
                b.HasMany(c => c.Models).WithOne(s => s.Make).HasForeignKey(x => x.MakeId);
            });          
            modelBuilder.Entity<User>(b =>
            {
                b.HasMany(c => c.PasswordResets).WithOne(s => s.User).HasForeignKey(x => x.UserId);
            });*/
            /*modelBuilder.Entity<Model>(b =>
            {
                b.HasOne(c => c.Make).WithMany(s => s.Models).HasForeignKey(x => x.MakeId);
            });
            modelBuilder.Entity<Equipment>(b =>
            {
                b.HasOne(c => c.Car).WithMany(s => s.Equipment).HasForeignKey(x => x.CarId);
            });
            modelBuilder.Entity<PasswordReset>(b =>
            {
                b.HasOne(c => c.User).WithMany(s => s.PasswordResets).HasForeignKey(x => x.UserId);
            });
            modelBuilder.Entity<Repair>(b =>
            {
                b.HasOne(c => c.Car).WithMany(s => s.Repairs).HasForeignKey(x => x.CarId);
            });
            modelBuilder.Entity<Shipping>(b =>
            {
                b.HasOne(c => c.Car).WithOne(s => s.Shipping).HasForeignKey<Shipping>(x => x.CarId);
            });
            modelBuilder.Entity<Summary>(b =>
            {
                b.HasOne(c => c.Car).WithOne(s => s.Summary).HasForeignKey<Summary>(x => x.CarId);
            });
            modelBuilder.Entity<Tracking>(b =>
            {
                b.HasOne(c => c.Car).WithOne(s => s.Tracking).HasForeignKey<Tracking>(x => x.CarId);
            });
            modelBuilder.Entity<TrackingImage>(b =>
            {
                b.HasOne(c => c.Tracking).WithMany(s => s.Images).HasForeignKey(x => x.TrackingId);
            });
            modelBuilder.Entity<CarImage>(b =>
            {
                b.HasOne(c => c.Car).WithMany(s => s.Images).HasForeignKey(x => x.CarId);
            });*/

        }
    }
}
