namespace CareerTracker.Kernel;

public sealed record TokenPair(string AccessToken, string RefreshToken, TimeSpan AccessLifetime);
