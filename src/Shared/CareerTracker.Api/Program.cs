using CareerTracker.Api.Extensions;
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

var app = builder.Build();

// 1. Применяем миграции БД (только если не в режиме строгой production-стабильности)
if (app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("ApplyMigrationsOnStart"))
{
    await app.ApplyDatabaseMigrationsAsync();
}

// 2. Настраиваем Middleware Pipeline
app.ConfigurePipeline();
await app.RunAsync();
