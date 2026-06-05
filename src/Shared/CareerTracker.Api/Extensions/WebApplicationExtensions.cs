using Microsoft.EntityFrameworkCore;
using Serilog;
using CareerTracker.Identity;
using CareerTracker.CareerPath;
using CareerTracker.JobAggregator;
using CareerTracker.Mentorship;

namespace CareerTracker.Api.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Настраивает конвейер обработки HTTP-запросов (Middleware).
    /// Порядок важен!
    /// </summary>
    public static void ConfigurePipeline(this WebApplication app)
    {
        // 1. Обработка ошибок (должно быть первым, чтобы ловить ошибки в других middleware)
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Career Tracker API V1");
            });
        }
        else
        {
            // В продакшене используем наш кастомный GlobalExceptionHandler, зарегистрированный в DI
            app.UseExceptionHandler();
            app.UseHsts();
        }

        // 2. Логирование HTTP-запросов (Serilog)
        app.UseSerilogRequestLogging();

        // 3. Безопасность и стандартные middleware
        app.UseHttpsRedirection();

        // 4. Аутентификация и Авторизация
        app.UseAuthentication();
        app.UseAuthorization();

        // 5. Маршрутизация
        app.MapControllers(); // Для традиционных контроллеров (например, Admin Panel API)

        // Здесь можно добавить маппинг Minimal API endpoints, если они не были замаплены в Program.cs
        app.MapIdentityEndpoints();
        app.MapCareerPathEndpoints();
        app.MapMentorshipEndpoints();
        app.MapJobAggregatorEndpoints();
    }

    /// <summary>
    /// Применяет ожидающие миграции EF Core к базе данных при запуске.
    /// Удобно для разработки и контейнеризации (Docker).
    /// </summary>
    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        // Получаем все DbContext из контейнера зависимостей
        // В нашем случае это может быть один общий контекст или несколько, если модули изолированы
        var dbContexts = scope.ServiceProvider.GetServices<DbContext>();

        foreach (var context in dbContexts)
        {
            try
            {
                // Проверяем, есть ли ожидающие миграции
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    Console.WriteLine($"Applying migrations for {context.GetType().Name}...");
                    await context.Database.MigrateAsync();
                    Console.WriteLine($"Migrations applied for {context.GetType().Name}.");
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку, но не падаем, если БД недоступна (опционально)
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the database: {ContextName}", context.GetType().Name);

                // В продакшене лучше выбросить исключение, чтобы контейнер перезапустился
                throw;
            }
        }
    }
}
