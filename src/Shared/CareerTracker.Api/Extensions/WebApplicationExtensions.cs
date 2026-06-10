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
}
