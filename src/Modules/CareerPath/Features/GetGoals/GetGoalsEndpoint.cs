using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.CareerPath.Features.GetGoals;

public static class GetGoalsEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/goals", async (int userId, ISender mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetGoalsQuery(userId), ct);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization()
        .WithName("GetGoals")
        .WithOpenApi();
    }
}
