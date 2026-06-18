using CareerTracker.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Настройка Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)    
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbContextTypes = builder.Services
    .Where(descriptor =>
        typeof(DbContext).IsAssignableFrom(descriptor.ServiceType) 
        && descriptor.ServiceType.IsClass 
        && !descriptor.ServiceType.IsAbstract)
    .Select(descriptor => descriptor.ServiceType)
    .Distinct()
    .ToList();

var app = builder.Build();

// 1. Применяем миграции БД (только если не в режиме строгой production-стабильности)
if (app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("ApplyMigrationsOnStart"))
{
    await app.MigrateDatabasesAsync(dbContextTypes);

    /*using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

        var admin = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@careertracker.local");
        if (admin != null)
        {
            // 1. Генерируем НАСТОЯЩИЙ, валидный хэш для пароля "Admin123!"
            var realHash = hasher.HashPassword(admin, "Admin123!");
            admin.SetPasswordHash(realHash);
            await context.SaveChangesAsync();

            Console.WriteLine("");
        }
    }*/
}

// 2. Настраиваем Middleware Pipeline
app.ConfigurePipeline();
await app.RunAsync();

public partial class Program { }
