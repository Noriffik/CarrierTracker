using CareerTracker.CareerPath.Data;
using CareerTracker.CareerPath.Features.CreateGoal;
using CareerTracker.CareerPath.Features.GetGoals;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CareerTracker.CareerPath;

public static class CareerPathEndpointExtensions
{
    public static IServiceCollection AddCareerPathModule(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("Default");

        services.AddDbContext<CareerPathDbContext>(opt =>
            opt.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsAssembly(typeof(CareerPathDbContext).Assembly.FullName)));

        return services;
    }

    public static void MapCareerPathEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/career-path").WithTags("CareerPath");
        CreateGoalEndpoint.MapEndpoint(group);
        GetGoalsEndpoint.MapEndpoint(group);
    }
}