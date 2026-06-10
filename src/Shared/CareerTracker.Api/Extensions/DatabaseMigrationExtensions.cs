using Microsoft.EntityFrameworkCore;

namespace CareerTracker.Api.Extensions;

public static class DatabaseMigrationExtensions
{
    public static async Task MigrateDatabasesAsync(this WebApplication app, IEnumerable<Type> dbContextTypes)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Начало применения миграций базы данных...");

        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        foreach (var contextType in dbContextTypes)
        {
            var contextName = contextType.Name;
            logger.LogInformation("🔍 Проверка миграций для {ContextName}...", contextName);

            try
            {
                // ✅ Динамически получаем экземпляр DbContext из DI по его типу
                var context = (DbContext)serviceProvider.GetRequiredService(contextType);

                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

                if (pendingMigrations.Any())
                {
                    logger.LogInformation("⏳ Применение {Count} миграций для {ContextName}...",
                        pendingMigrations.Count(), contextName);

                    await context.Database.MigrateAsync();

                    logger.LogInformation("✅ Миграции для {ContextName} успешно применены.", contextName);
                }
                else
                {
                    logger.LogInformation("✔️ База данных {ContextName} актуальна. Миграции не требуются.", contextName);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ Критическая ошибка при применении миграций для {ContextName}. Приложение не может быть запущено.", contextName);
                throw; // Fail-fast: останавливаем запуск при ошибке БД
            }
        }

        logger.LogInformation("🎉 Все миграции базы данных успешно применены.");
    }
}
