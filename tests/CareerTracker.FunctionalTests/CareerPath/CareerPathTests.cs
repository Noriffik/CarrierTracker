using CareerTracker.FunctionalTests.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace CareerTracker.FunctionalTests.CareerPath;

public class CareerPathTests : IClassFixture<TestWebAppFactory>
{
    private readonly HttpClient _client;

    public CareerPathTests(TestWebAppFactory factory) => _client = factory.CreateClient();

    [Fact]
    public async Task CreateGoal_AuthenticatedUser_ReturnsCreated()
    {
        // Подготовка: Регистрация и Логин
        var email = $"test-{Guid.NewGuid():N}@example.com";
        var userId = await _client.RegisterUserAsync(email);
        var token = await _client.GetAuthTokenAsync(email);
        _client.SetBearerToken(token);

        // Действие: Создание цели
        var goalData = new { UserId = userId, Title = "Learn Dapper", Description = "Master ORM", Deadline = (DateTime?)null };
        var response = await _client.PostAsJsonAsync("/api/career-path/goals", goalData, cancellationToken: TestContext.Current.CancellationToken);

        // 3. Проверка
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}