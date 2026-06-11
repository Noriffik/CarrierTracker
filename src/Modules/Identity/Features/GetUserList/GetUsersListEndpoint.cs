using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.Identity.Features.GetUserList;

public static class GetUsersListEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (
            [AsParameters] GetUsersListQuery query,
            ISender mediator,
            CancellationToken ct) =>
        {
            // Валидация пагинации на уровне endpoint
            if (query.Page < 1) query = query with { Page = 1 };
            if (query.PageSize < 1 || query.PageSize > 100) query = query with { PageSize = 20 };

            var result = await mediator.Send(query, ct);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin"))
        .WithName("GetUsersList")
        .WithTags("Identity-Admin");
    }
}