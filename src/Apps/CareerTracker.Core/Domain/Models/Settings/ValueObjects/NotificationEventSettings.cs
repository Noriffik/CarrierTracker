namespace CareerTracker.Core.Domain.Models.Settings.ValueObjects;

public struct NotificationEventSettings
{
    /// <summary>
    /// Indicates whether the notification event is enabled.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Specifies the time at which the notification event is scheduled to occur.
    /// </summary>
    public TimeSpan ScheduledTime { get; set; }
}