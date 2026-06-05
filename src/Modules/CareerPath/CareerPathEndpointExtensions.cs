using CareerTracker.CareerPath.Features.CompleteLesson;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.CareerPath;

public static class CareerPathEndpointExtensions
{
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