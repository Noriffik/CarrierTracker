namespace CareerTracker.Kernel.Services;

public interface IPasswordResetTokenGenerator
{
    string Generate(int userId);
}
