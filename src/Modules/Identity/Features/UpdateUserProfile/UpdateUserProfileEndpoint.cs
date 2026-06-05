using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.Identity.Features.UpdateUserProfile;

public static class UpdateUserProfileEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/profile", async (UpdateUserProfileCommand command, ISender mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return result.IsSuccess
                ? Results.NoContent()
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("UpdateUserProfile")
        .WithTags("Identity")
        .WithOpenApi()
        .RequireAuthorization(); // Пользователь может менять только свой профиль
    }
}