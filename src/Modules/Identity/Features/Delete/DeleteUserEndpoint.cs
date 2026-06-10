using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.Identity.Features.Delete;

public static class DeleteUserEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/users/{userId:int}/delete", async (int userId, ISender mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new DeleteUserCommand(userId), ct);
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin"))
        .WithName("DeleteUser")
        .WithTags("Identity-Admin");
    }
}
