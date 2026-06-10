using System.Security.Cryptography;

namespace CareerTracker.Kernel.Services;

public interface IPasswordResetTokenGenerator
{
    string Generate();
}

public class PasswordResetTokenGenerator : IPasswordResetTokenGenerator
{
    public string Generate()
    {
        var bytes = new byte[32];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").TrimEnd('=');
    }
}
