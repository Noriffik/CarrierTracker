using CareerTracker.CareerPath.Domain;
using Microsoft.EntityFrameworkCore;

namespace CareerTracker.CareerPath.Data;

public class CareerPathDbContext : DbContext
{
    public DbSet<CareerGoal> Goals => Set<CareerGoal>();

    public CareerPathDbContext(DbContextOptions<CareerPathDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CareerGoal>(b =>
        {
            b.HasKey(g => g.Id);
            b.Property(g => g.Title).HasMaxLength(200).IsRequired();
            b.Property(g => g.Description).HasMaxLength(2000);
            b.Property(g => g.Status).HasConversion<string>().HasMaxLength(20);

            // Индекс для быстрого поиска целей конкретного пользователя
            b.HasIndex(g => g.UserId);

            b.ToTable("CareerGoals");
        });
    }
}
