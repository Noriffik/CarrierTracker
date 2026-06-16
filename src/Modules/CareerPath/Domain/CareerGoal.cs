namespace CareerTracker.CareerPath.Domain;

public sealed class CareerGoal
{
    private CareerGoal() { } // EF Core

    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = string.Empty;
    public GoalStatus Status { get; private set; }
    public DateTime? Deadline { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public static CareerGoal Create(int userId, string title, string description, DateTime? deadline)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required");

        return new CareerGoal
        {
            UserId = userId,
            Title = title.Trim(),
            Description = description?.Trim() ?? string.Empty,
            Status = GoalStatus.Planned,
            Deadline = deadline,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateDetails(string title, string description, DateTime? deadline)
    {
        Title = title.Trim();
        Description = description?.Trim() ?? string.Empty;
        Deadline = deadline;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsInProgress() => Status = GoalStatus.InProgress;
    public void MarkAsCompleted() => Status = GoalStatus.Completed;
    public void MarkAsArchived() => Status = GoalStatus.Archived;
}

public enum GoalStatus { Planned = 0, InProgress = 1, Completed = 2, Archived = 3 }