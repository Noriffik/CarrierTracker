using CareerTracker.CareerPath.Data;
using CareerTracker.CareerPath.Features.CompleteLesson;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

namespace CareerTracker.CareerPath;

public static class CareerPathEndpointExtensions
{

    public static IServiceCollection AddCareerPathModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        // Регистрация Dapper connection
        services.AddScoped<IDbConnection>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            return new NpgsqlConnection(config.GetConnectionString("Default"));
        });
        services.AddDbContext<CareerPathDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsAssembly(typeof(CareerPathDbContext).Assembly.FullName)));
        return services;
    }

    public static void MapCareerPathEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/careerpath")
                       .WithTags("CareerPath");

        // Регистрация эндпоинтов из слайсов
        CompleteLessonEndpoint.MapEndpoint(group);
        //RegisterUser.Endpoint.MapEndpoint(group);
        //Login.Endpoint.MapEndpoint(group);
        //GetProfile.Endpoint.MapEndpoint(group);
    }
}