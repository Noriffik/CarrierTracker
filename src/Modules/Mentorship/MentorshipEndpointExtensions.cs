using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.Mentorship;

public static class MentorshipEndpointExtensions
{
    public static void MapMentorshipEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/mentorship")
                       .WithTags("Mentorship");

        // Регистрация эндпоинтов из слайсов
        //RegisterUser.Endpoint.MapEndpoint(group);
        //Login.Endpoint.MapEndpoint(group);
        //GetProfile.Endpoint.MapEndpoint(group);
    }
}
