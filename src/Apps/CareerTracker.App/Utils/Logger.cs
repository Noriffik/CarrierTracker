using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTracker.App.Utils;

public static class Logger
{
    public static void LogInfo(string message)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine(message);
#else
		Console.WriteLine(message);
#endif
    }

    public static void LogEvent(string name)
    {
        LogInfo(name);

#if RELEASE
        var analyticsService = App.ServiceProvider.GetService<IAnalyticsService>();
        analyticsService?.LogEvent(name);
#endif
    }

    public static void LogException(Exception exception)
    {
        LogInfo(exception.ToString());

#if RELEASE
        var crashlyticsService = App.ServiceProvider.GetService<ICrashlyticsService>();
        crashlyticsService?.LogException(exception);
#endif
    }
}