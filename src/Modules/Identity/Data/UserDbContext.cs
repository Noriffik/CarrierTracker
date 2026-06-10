using CareerTracker.Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace CareerTracker.Identity.Data;

public sealed class UserDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserProfile> Profiles => Set<UserProfile>();
    public DbSet<PasswordResetRequest> PasswordResetRequests => Set<PasswordResetRequest>();

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            b.HasKey(u => u.Id);
            b.Property(u => u.Email).HasMaxLength(256).IsRequired();
            b.HasIndex(u => u.Email).IsUnique().HasFilter("\"IsActive\" = TRUE AND \"IsDeleted\" = FALSE");
            b.Property(u => u.PasswordHash).HasMaxLength(512).IsRequired();
            b.Property(u => u.Role).HasConversion<string>();
            b.Property(u => u.IsDeleted).HasDefaultValue(false);

            b.HasQueryFilter(u => !u.IsDeleted);

            b.ToTable("Users");
        });

        modelBuilder.Entity<UserProfile>(b =>
        {
            b.HasKey(p => p.Id);
            b.HasOne<User>().WithMany().HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);

            // Храним Attributes как JSONB
            b.Property(p => p.Attributes)
                .HasColumnType("jsonb")
                .HasDefaultValueSql("'{}'::jsonb");

            b.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
            b.Property(p => p.LastName).HasMaxLength(100).IsRequired();
            b.Property(p => p.PhoneNumber).HasMaxLength(20);
            b.Property(p => p.City).HasMaxLength(100);

            b.ToTable("UserProfiles");
        });

        modelBuilder.Entity<PasswordResetRequest>(b =>
        {
            b.HasKey(r => r.Id);
            b.HasIndex(r => r.TokenHash).IsUnique();
            b.Property(r => r.TokenHash).HasMaxLength(64).IsRequired();
            b.Property(r => r.IsUsed).HasDefaultValue(false);
            b.ToTable("PasswordResetRequests");
        });
    }
}
