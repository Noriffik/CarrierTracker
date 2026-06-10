using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.Identity.Features.DeactivateUser;

public static class DeactivateUserEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/users/{userId:int}/deactivate", async (int userId, ISender mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new DeactivateUserCommand(userId), ct);
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin"))
        .WithName("DeactivateUser")
        .WithTags("Identity-Admin");
    }
}