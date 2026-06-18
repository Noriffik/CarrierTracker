using CareerTracker.CareerPath.Data;
using CareerTracker.Identity.Data;
using CareerTracker.Kernel.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Data.Common;

namespace CareerTracker.FunctionalTests.Fixtures;

public class TestWebAppFactory : WebApplicationFactory<Program>
{
    // Хранилище для перехвата токенов сброса пароля и других уведомлений
    public ConcurrentDictionary<string, string> CapturedEmails { get; } = new();

    private static readonly string _uniqueDbName = $"file:testdb-{Guid.NewGuid():N}.db?mode=memory&cache=shared";
    private static readonly SqliteConnection _sharedConnection = new SqliteConnection($"Data Source={_uniqueDbName}");

    // Флаг, чтобы миграции применились только один раз за все тесты
    private static bool _isDatabaseInitialized = false;
    private static readonly object _lock = new();

    static TestWebAppFactory()
    {
        _sharedConnection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");


        builder.ConfigureServices(services =>
        {
            var efDescriptors = services.Where(d =>
                d.ServiceType.FullName?.Contains("Microsoft.EntityFrameworkCore") == true ||
                d.ImplementationType?.FullName?.Contains("Microsoft.EntityFrameworkCore") == true ||
                d.ServiceType.FullName?.Contains("Npgsql") == true
            ).ToList();

            foreach (var descriptor in efDescriptors) services.Remove(descriptor);

            services.RemoveAll<UserDbContext>();
            services.RemoveAll<CareerPathDbContext>();

            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlite(_sharedConnection));

            services.AddDbContext<CareerPathDbContext>(options =>
                options.UseSqlite(_sharedConnection));

            services.RemoveAll(typeof(IEmailSender));
            services.AddSingleton<IEmailSender>(new CaptureEmailSender(CapturedEmails));

            services.AddSingleton<ILoggerFactory>(new NullLoggerFactory());
        });
    }

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);

        if (!_isDatabaseInitialized)
        {
            lock (_lock)
            {
                if (!_isDatabaseInitialized)
                {
                    InitializeDatabase();
                    _isDatabaseInitialized = true;
                }
            }
        }
    }

    private void InitializeDatabase()
    {
        using var scope = Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var connection = serviceProvider.GetRequiredService<UserDbContext>().Database.GetDbConnection();

        using var cmd = connection.CreateCommand();

        cmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS ""Users"" (
            ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_Users"" PRIMARY KEY AUTOINCREMENT,
            ""Email"" TEXT NOT NULL,
            ""PasswordHash"" TEXT NOT NULL,
            ""Role"" TEXT NOT NULL,
            ""IsActive"" INTEGER NOT NULL DEFAULT 1,
            ""IsDeleted"" INTEGER NOT NULL DEFAULT 0,
            ""CreatedAt"" TEXT NOT NULL,
            ""UpdatedAt"" TEXT NULL
        );
        CREATE UNIQUE INDEX IF NOT EXISTS ""IX_Users_Email"" ON ""Users"" (""Email"");
    ";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS ""UserProfiles"" (
            ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_UserProfiles"" PRIMARY KEY AUTOINCREMENT,
            ""UserId"" INTEGER NOT NULL,
            ""FirstName"" TEXT NOT NULL,
            ""LastName"" TEXT NOT NULL,
            ""PhoneNumber"" TEXT NULL,
            ""City"" TEXT NULL,
            ""Attributes"" TEXT NULL,
            ""CreatedAt"" TEXT NOT NULL,
            ""UpdatedAt"" TEXT NULL,
            CONSTRAINT ""FK_UserProfiles_Users_UserId"" FOREIGN KEY (""UserId"") REFERENCES ""Users"" (""Id"") ON DELETE CASCADE
        );
    ";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS ""PasswordResetRequests"" (
            ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_PasswordResetRequests"" PRIMARY KEY AUTOINCREMENT,
            ""UserId"" INTEGER NOT NULL,
            ""TokenHash"" TEXT NOT NULL,
            ""ExpiresAt"" TEXT NOT NULL,
            ""IsUsed"" INTEGER NOT NULL DEFAULT 0,
            ""CreatedAt"" TEXT NOT NULL,
            CONSTRAINT ""FK_PasswordResetRequests_Users_UserId"" FOREIGN KEY (""UserId"") REFERENCES ""Users"" (""Id"") ON DELETE CASCADE
        );
    ";
        cmd.ExecuteNonQuery();
     
        cmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS ""CareerGoals"" (
            ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_CareerGoals"" PRIMARY KEY AUTOINCREMENT,
            ""UserId"" INTEGER NOT NULL,
            ""Title"" TEXT NOT NULL,
            ""Description"" TEXT NULL,
            ""Status"" TEXT NOT NULL,
            ""Deadline"" TEXT NULL,
            ""CreatedAt"" TEXT NOT NULL,
            ""UpdatedAt"" TEXT NULL
        );
        CREATE INDEX IF NOT EXISTS ""IX_CareerGoals_UserId"" ON ""CareerGoals"" (""UserId"");
    ";
        cmd.ExecuteNonQuery();

        Console.WriteLine("✅ All tables manually created successfully.");
    }
}
