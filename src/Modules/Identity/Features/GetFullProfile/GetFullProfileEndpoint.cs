using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.Identity.Features.GetFullProfile;

public static class GetFullProfileEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/fullprofile", async (
            [AsParameters] GetFullProfileQuery query,
            ISender mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(query, ct);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("GetFullProfile")
        .WithTags("Identity")
        .WithOpenApi()
        .RequireAuthorization();
    }
}