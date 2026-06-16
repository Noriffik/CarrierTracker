using CareerTracker.Identity.Features.Login;
using CareerTracker.Identity.Features.RegisterUser;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CareerTracker.FunctionalTests.Fixtures;

public static class TestHelpers
{
    /// <summary>
    /// Регистрирует нового пользователя и возвращает его ID.
    /// </summary>
    public static async Task<int> RegisterUserAsync(this HttpClient client, string email, string password = "TestPass123!")
    {
        var response = await client.PostAsJsonAsync("/api/identity/register",
            new RegisterUserCommand(email, password, CareerTracker.Identity.Domain.UserRole.Graduate));

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
        return body!["id"];
    }

    /// <summary>
    /// Логинится и возвращает JWT токен.
    /// </summary>
    public static async Task<string> GetAuthTokenAsync(this HttpClient client, string email, string password = "TestPass123!")
    {
        var response = await client.PostAsJsonAsync("/api/identity/login",
            new LoginCommand(email, password));

        response.EnsureSuccessStatusCode();
        var tokens = await response.Content.ReadFromJsonAsync<AuthTokensDto>();
        return tokens!.AccessToken;
    }

    /// <summary>
    /// Устанавливает Bearer токен для всех последующих запросов клиента.
    /// </summary>
    public static void SetBearerToken(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}