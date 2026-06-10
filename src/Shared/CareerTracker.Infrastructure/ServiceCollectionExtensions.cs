using CareerTracker.CareerPath;
using CareerTracker.Identity;
using CareerTracker.Infrastructure.Auth;
using CareerTracker.Kernel.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

namespace CareerTracker.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IPasswordResetTokenGenerator, PasswordResetTokenGenerator>();
        services.AddIdentityModule(configuration);
        services.AddCareerPathModule(configuration);

        return services;
    }

    public static IServiceCollection AddCareerTrackerModules(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName!.StartsWith("CareerTracker."))
            .ToArray();

        // Автоматическая регистрация всех MediatR handlers и Validators
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        services.AddValidatorsFromAssemblies(assemblies);

        // Регистрация Dapper connection
        services.AddScoped<IDbConnection>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            return new NpgsqlConnection(config.GetConnectionString("Default"));
        });

        return services;
    }
}
