using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CareerTracker.CareerPath.Data.DesignTime;

public sealed class CareerPathDbContextFactory : IDesignTimeDbContextFactory<CareerPathDbContext>
{
    public CareerPathDbContext CreateDbContext(string[] args)
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

        var optionsBuilder = new DbContextOptionsBuilder<CareerPathDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            // Настройки для миграций
            npgsqlOptions.MigrationsAssembly(typeof(CareerPathDbContext).Assembly.FullName);
            npgsqlOptions.CommandTimeout(60);
        });

        // Включаем sensitive data logging только для design-time
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();

        return new CareerPathDbContext(optionsBuilder.Options);
    }
}
