using CareerTracker.CareerPath.Features.CreateGoal;
using CareerTracker.FunctionalTests.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace CareerTracker.FunctionalTests.CareerPath;

public class GoalEndpointsTests : IClassFixture<TestWebAppFactory>
{
    private readonly HttpClient _client;
    public GoalEndpointsTests(TestWebAppFactory factory) => _client = factory.CreateClient();

    [Fact]
    public async Task CreateGoal_ValidData_ReturnsCreated()
    {
        // Подготовка: Регистрация и Логин
        var email = $"test-{Guid.NewGuid():N}@example.com";
        await _client.RegisterUserAsync(email);
        var token = await _client.GetAuthTokenAsync(email);
        _client.SetBearerToken(token);

        // Для простоты предположим, что у нас есть тестовый юзер с ID 1

        var cmd = new CreateGoalCommand(1, "New Goal", "Description", null);
        var response = await _client.PostAsJsonAsync("/api/career-path/goals", cmd, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
