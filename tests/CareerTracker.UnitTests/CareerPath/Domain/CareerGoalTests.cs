using CareerTracker.CareerPath.Domain;

namespace CareerTracker.UnitTests.CareerPath.Domain;

public class CareerGoalTests
{
    [Fact]
    public void Create_ValidData_ReturnsPlannedGoal()
    {
        var goal = CareerGoal.Create(1, "Learn C#", "Basic syntax", DateTime.UtcNow.AddMonths(1));

        Assert.Equal("Learn C#", goal.Title);
        Assert.Equal(GoalStatus.Planned, goal.Status);
        Assert.NotNull(goal.CreatedAt);
    }

    [Fact]
    public void MarkAsCompleted_UpdatesStatus()
    {
        var goal = CareerGoal.Create(1, "Test", "", null);
        goal.MarkAsCompleted();
        Assert.Equal(GoalStatus.Completed, goal.Status);
    }
}
