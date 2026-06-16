using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTracker.App.Helpers;

public static class ResourceHelper
{
    public static string GetText(string key)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var value) == true) return value as string ?? key;
        return key;
    }
}
