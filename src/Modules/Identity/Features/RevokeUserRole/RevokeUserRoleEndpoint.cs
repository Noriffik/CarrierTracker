using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.Identity.Features.RevokeUserRole;

public static class RevokeUserRoleEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        // Используем DELETE, так как мы "удаляем" специальные полномочия
        app.MapDelete("/users/{userId:int}/role", async (
            int userId,
            ISender mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new RevokeUserRoleCommand(userId), ct);
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin"))
        .WithName("RevokeUserRole")
        .WithTags("Identity-Admin");
    }
}
