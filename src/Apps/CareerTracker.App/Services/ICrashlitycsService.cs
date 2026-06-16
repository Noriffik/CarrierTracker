namespace CareerTracker.App.Services;

public interface ICrashlyticsService
{
    void InitCrashDetection();

    void LogException(Exception exception, Dictionary<string, string> properties = null);
}
