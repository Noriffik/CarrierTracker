using CareerTracker.Core.Domain.SharedKernels;

namespace CareerTracker.Core.Domain.Models.Settings.ValueObjects;

public class AuthenticationSettings : ValueObject
{
    /// <summary>
    /// Indicates whether authentication is enabled in the application.
    /// </summary>
    public required bool IsAuthenticationEnabled { get; set; }

    /// <summary>
    /// Determines whether biometric authentication is enabled in the app.
    /// </summary>
    public required bool IsBiometricAuthEnabled { get; set; }

    /// <summary>
    /// A hashed authentication code to the app.
    /// </summary>
    public required string? PasswordHash { get; set; }
}
