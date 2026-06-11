using CareerTracker.Identity.Data;
using CareerTracker.Identity.Domain;
using CareerTracker.Identity.Features.ActivateUser;
using CareerTracker.Identity.Features.ChangeUserRole;
using CareerTracker.Identity.Features.DeactivateUser;
using CareerTracker.Identity.Features.Delete;
using CareerTracker.Identity.Features.GetFullProfile;
using CareerTracker.Identity.Features.GetUserList;
using CareerTracker.Identity.Features.GetUserProfile;
using CareerTracker.Identity.Features.Login;
using CareerTracker.Identity.Features.RegisterUser;
using CareerTracker.Identity.Features.RequestPasswordReset;
using CareerTracker.Identity.Features.ResetPassword;
using CareerTracker.Identity.Features.RevokeUserRole;
using CareerTracker.Identity.Features.UpdateUserProfile;
using CareerTracker.Identity.Infrastructure;
using CareerTracker.Kernel.Common;
using CareerTracker.Kernel.Services;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;
using System.Reflection;

namespace CareerTracker.Identity;

public static class IdentityEndpointExtensions
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddSingleton<ITimeProvider, SystemTimeProvider>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        var connectionString = configuration.GetConnectionString("Default");        

        // Регистрация Dapper connection
        services.AddScoped<IDbConnection>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            return new NpgsqlConnection(config.GetConnectionString("Default"));
        });
        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName)));
        return services;
    }

    /// <summary>
    /// Регистрирует все endpoint'ы модуля Identity.
    /// Вызывается из WebApplicationExtensions.ConfigurePipeline()
    /// </summary>
    public static void MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/identity")
                       .WithTags("Identity");

        // Регистрация эндпоинтов из слайсов
        RegisterUserEndpoint.MapEndpoint(group);
        LoginEndpoint.MapEndpoint(group);
        ActivateUserEndpoint.MapEndpoint(group);
        DeactivateUserEndpoint.MapEndpoint(group);
        DeleteUserEndpoint.MapEndpoint(group);
        RevokeUserRoleEndpoint.MapEndpoint(group);
        RequestPasswordResetEndpoint.MapEndpoint(group);
        ResetPasswordEndpoint.MapEndpoint(group);
        GetUserProfileEndpoint.MapEndpoint(group);
        GetUsersListEndpoint.MapEndpoint(group);
        GetFullProfileEndpoint.MapEndpoint(group);
        UpdateUserProfileEndpoint.MapEndpoint(group);
        ChangeUserRoleEndpoint.MapEndpoint(group);
    }
}
