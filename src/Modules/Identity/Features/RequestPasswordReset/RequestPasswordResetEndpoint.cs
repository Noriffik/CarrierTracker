using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.Identity.Features.RequestPasswordReset;

public static class RequestPasswordResetEndpoint
{
    private const string IfAccountExistsSendEmail = "Если аккаунт существует, инструкция отправлена на email";

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/password-reset/request", async (
            RequestPasswordResetCommand command,
            ISender mediator,
            CancellationToken ct) =>
        {
            _ = await mediator.Send(command, ct);
            // Всегда 200 OK — не даем информации о существовании аккаунта
            return Results.Ok(new { message = IfAccountExistsSendEmail });
        })
        .WithName("RequestPasswordReset")
        .WithTags("Identity")
        .WithOpenApi()
        .AllowAnonymous()
        .RequireRateLimiting("password-reset"); // Rate limit: макс 3 запроса в час на IP
    }
}