using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CareerTracker.Identity.Data.DesignTime;

public sealed class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        // Путь к appsettings.json относительно startup project (CareerTracker.API)
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "CareerTracker.API");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException(
                "Connection string 'Default' not found. " +
                "Ensure appsettings.json contains a valid PostgreSQL connection string.");

        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            // Настройки для миграций
            npgsqlOptions.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName);
            npgsqlOptions.CommandTimeout(60);
        });

        // Включаем sensitive data logging только для design-time
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();

        return new UserDbContext(optionsBuilder.Options);
    }
}
