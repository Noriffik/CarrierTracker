using CareerTracker.FunctionalTests.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace CareerTracker.FunctionalTests.CareerPath;

public class CareerPathTests : IClassFixture<TestWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly TestWebAppFactory _factory;

    public CareerPathTests(TestWebAppFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateGoal_AuthenticatedUser_ReturnsCreated()
    {
        // 1. Подготовка: Регистрация и Логин
        var email = $"test-{Guid.NewGuid():N}@example.com";
        await _client.RegisterUserAsync(email);
        var token = await _client.GetAuthTokenAsync(email);
        _client.SetBearerToken(token);

        // 2. Действие: Создание цели
        var goalData = new { UserId = 1, Title = "Learn Dapper", Description = "Master ORM", Deadline = (DateTime?)null };
        var response = await _client.PostAsJsonAsync("/api/career-path/goals", goalData);

        // 3. Проверка
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}