using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.CareerPath.Features.CompleteLesson;

public static class CompleteLessonEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/career/lessons/complete", async (
            CompleteLessonCommand command,
            ISender mediator) =>
        {
            var result = await mediator.Send(command);
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        })
            .WithTags("Career", "Lessons")
            .WithOpenApi()
        .RequireAuthorization();
    }
}
