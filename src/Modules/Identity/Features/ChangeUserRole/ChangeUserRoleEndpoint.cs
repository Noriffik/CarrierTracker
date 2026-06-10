using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.Identity.Features.ChangeUserRole;

public static class ChangeUserRoleEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/users/{userId:int}/role", async (
            int userId,
            ChangeUserRoleCommand command,
            ISender mediator,
            CancellationToken ct) =>
        {
            if (command.UserId != userId)
                return Results.BadRequest(new { error = "UserId в пути и теле запроса не совпадают" });

            var result = await mediator.Send(command, ct);
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin")) // 🔒 Только Admin
        .WithName("ChangeUserRole")
        .WithTags("Identity-Admin");
    }
}