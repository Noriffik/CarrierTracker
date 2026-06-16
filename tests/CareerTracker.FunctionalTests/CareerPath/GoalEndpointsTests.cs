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
        // Arrange: Сначала нужно залогиниться или использовать мокированный токен админа/юзера
        // Для простоты предположим, что у нас есть тестовый юзер с ID 1

        var cmd = new CreateGoalCommand(1, "New Goal", "Description", null);
        var response = await _client.PostAsJsonAsync("/api/career-path/goals", cmd);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
