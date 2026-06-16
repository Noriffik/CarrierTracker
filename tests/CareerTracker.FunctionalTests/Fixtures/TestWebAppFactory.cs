using CareerTracker.CareerPath.Data;
using CareerTracker.Identity.Data;
using CareerTracker.Kernel.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
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

    // Флаг, чтобы миграции применились только один раз за все тесты
    private static bool _isDatabaseInitialized = false;
    private static readonly object _lock = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // 1. Удаляем реальные регистрации DbContext
            RemoveDbContext<UserDbContext>(services);
            RemoveDbContext<CareerPathDbContext>(services);

            // 2. Настраиваем In-Memory SQLite
            var connection = CreateInMemorySqliteConnection();

            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlite(connection));

            services.AddDbContext<CareerPathDbContext>(options =>
                options.UseSqlite(connection));

            // 3. Подменяем внешние сервисы на заглушки
            services.RemoveAll(typeof(IEmailSender));
            services.AddSingleton<IEmailSender>(new CaptureEmailSender(CapturedEmails));

            // 4. Отключаем лишнее логирование в консоль во время тестов
            services.AddSingleton<ILoggerFactory>(new NullLoggerFactory());
        });
    }

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);

        // Инициализируем БД при первом запуске
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

    private static DbConnection CreateInMemorySqliteConnection()
    {
        var connection = new Microsoft.Data.Sqlite.SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    }

    private void InitializeDatabase()
    {
        using var scope = Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        // Применяем миграции для всех контекстов
        var contexts = new List<DbContext>
        {
            serviceProvider.GetRequiredService<UserDbContext>(),
            serviceProvider.GetRequiredService<CareerPathDbContext>()
        };

        foreach (var context in contexts)
        {
            context.Database.EnsureCreated(); // Для SQLite EnsureCreated быстрее и проще чем Migrate
            // Если нужны именно миграции, используйте: await context.Database.MigrateAsync();
        }
    }

    private static void RemoveDbContext<T>(IServiceCollection services) where T : DbContext
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<T>));
        if (descriptor != null) services.Remove(descriptor);
    }
}
