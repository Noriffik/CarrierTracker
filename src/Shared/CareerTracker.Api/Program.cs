using CareerTracker.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Настройка Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
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
        typeof(DbContext).IsAssignableFrom(descriptor.ServiceType) && // Является ли тип DbContext или его наследником
        descriptor.ServiceType.IsClass &&                             // Это класс (не интерфейс)
        !descriptor.ServiceType.IsAbstract)                           // Не абстрактный класс
    .Select(descriptor => descriptor.ServiceType)
    .Distinct()                                                       // Убираем дубликаты, если тип зарегистрирован несколько раз
    .ToList();

var app = builder.Build();

// 1. Применяем миграции БД (только если не в режиме строгой production-стабильности)
if (app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("ApplyMigrationsOnStart"))
{
    await app.MigrateDatabasesAsync(dbContextTypes);
}

// 2. Настраиваем Middleware Pipeline
app.ConfigurePipeline();
await app.RunAsync();
