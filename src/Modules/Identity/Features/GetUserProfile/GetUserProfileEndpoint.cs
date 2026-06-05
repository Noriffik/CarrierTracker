using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.Identity.Features.GetUserProfile;

public static class GetUserProfileEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/profile", async (
            [AsParameters] GetUserProfileQuery query,
            ISender mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(query, ct);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("GetUserProfile")
        .WithTags("Identity")
        .WithOpenApi()
        .RequireAuthorization(); // Только авторизованные пользователи
    }
}