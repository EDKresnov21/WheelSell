using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WheelSell.DAL.Entities;

namespace WheelSell.DAL
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<Transmission> Transmissions { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Owner)
                .WithMany(u => u.Cars)
                .HasForeignKey(c => c.OwnerId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Brand)
                .WithMany(b => b.Cars)
                .HasForeignKey(c => c.BrandId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Model)
                .WithMany(m => m.Cars)
                .HasForeignKey(c => c.ModelId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.FuelType)
                .WithMany(f => f.Cars)
                .HasForeignKey(c => c.FuelTypeId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Transmission)
                .WithMany(t => t.Cars)
                .HasForeignKey(c => c.TransmissionId);

            modelBuilder.Entity<Car>()
                .HasMany(c => c.Photos)
                .WithOne(p => p.Car)
                .HasForeignKey(p => p.CarId);

            modelBuilder.Entity<Car>()
                .HasMany(c => c.Videos)
                .WithOne(v => v.Car)
                .HasForeignKey(v => v.CarId);
        }
    }
}