namespace CareerTracker.Kernel.Common;

public interface ITimeProvider
{
    DateTime UtcNow { get; }
}
