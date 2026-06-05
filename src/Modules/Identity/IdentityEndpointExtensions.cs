using CareerTracker.Identity.Features.GetFullProfile;
using CareerTracker.Identity.Features.GetUserProfile;
using CareerTracker.Identity.Features.Login;
using CareerTracker.Identity.Features.RegisterUser;
using CareerTracker.Identity.Features.RequestPasswordReset;
using CareerTracker.Identity.Features.ResetPassword;
using CareerTracker.Identity.Features.UpdateUserProfile;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CareerTracker.Identity;

public static class IdentityEndpointExtensions
{
    /// <summary>
    /// Регистрирует все endpoint'ы модуля Identity.
    /// Вызывается из WebApplicationExtensions.ConfigurePipeline()
    /// </summary>
    public static void MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/identity")
                       .WithTags("Identity");

        // Регистрация эндпоинтов из слайсов
        RegisterUserEndpoint.MapEndpoint(group);
        LoginEndpoint.MapEndpoint(group);
        RequestPasswordResetEndpoint.MapEndpoint(group);
        ResetPasswordEndpoint.MapEndpoint(group);
        GetUserProfileEndpoint.MapEndpoint(group);
        GetFullProfileEndpoint.MapEndpoint(group);
        UpdateUserProfileEndpoint.MapEndpoint(group);
    }
}
