using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.Identity.Features.ActivateUser;

public static class ActivateUserEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/users/{userId:int}/activate", async (int userId, ISender mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new ActivateUserCommand(userId), ct);
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin"))
        .WithName("ActivateUser")
        .WithTags("Identity-Admin");
    }
}