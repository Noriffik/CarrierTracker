using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.Identity.Features.ResetPassword;

public static class ResetPasswordEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/password-reset/confirm", async (
            ResetPasswordCommand command,
            ISender mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return result.IsSuccess
                ? Results.Ok(new { message = "Пароль успешно изменен" })
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("ResetPassword")
        .WithTags("Identity")
        .WithOpenApi()
        .AllowAnonymous();
    }
}