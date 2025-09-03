using Microsoft.EntityFrameworkCore;
using WheelSell.DAL.Entities;

namespace WheelSell.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Car> Cars { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка отношения: один User имеет много Cars
            modelBuilder.Entity<User>()
                .HasMany(u => u.Cars)
                .WithOne(c => c.Owner)
                .HasForeignKey(c => c.OwnerId);
        }
    }
}