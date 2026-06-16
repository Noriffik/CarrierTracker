using CareerTracker.Core.Domain.SharedKernels;

namespace CareerTracker.Core.Domain.Models.Settings.ValueObjects;

public class NotificationsSettings : ValueObject
{
    /// <summary>
    /// Indicates whether the notifications feature is enabled or disabled in the system.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Defines the settings of reminders related to transaction notifications.
    /// </summary>
    public NotificationEventSettings AddTransactionReminder { get; set; }
}
