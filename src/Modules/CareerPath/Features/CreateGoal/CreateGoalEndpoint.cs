using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.CareerPath.Features.CreateGoal;

public static class CreateGoalEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/goals", async (CreateGoalCommand cmd, ISender mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            return result.IsSuccess ? Results.Created($"/api/career-path/goals/{result.Value}", new { id = result.Value })
                                    : Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization()
        .WithName("CreateGoal")
        .WithOpenApi();
    }
}