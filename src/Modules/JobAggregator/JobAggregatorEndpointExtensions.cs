using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.JobAggregator;

public static class JobAggregatorEndpointExtensions
{
    public static void MapJobAggregatorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/jobs")
                       .WithTags("Jobs");

        // Регистрация эндпоинтов из слайсов
        //RegisterUser.Endpoint.MapEndpoint(group);
        //Login.Endpoint.MapEndpoint(group);
        //GetProfile.Endpoint.MapEndpoint(group);
    }
}
