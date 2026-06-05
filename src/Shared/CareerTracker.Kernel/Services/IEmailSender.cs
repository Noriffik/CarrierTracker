namespace CareerTracker.Kernel.Services;

public interface IEmailSender
{
    Task SendPasswordResetEmailAsync(string email, string resetToken);
}
