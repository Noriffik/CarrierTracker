using CareerTracker.Kernel.Services;
using System.Collections.Concurrent;

namespace CareerTracker.FunctionalTests.Fixtures;

public sealed class CaptureEmailSender : IEmailSender
{
    private readonly ConcurrentDictionary<string, string> _store;

    public CaptureEmailSender(ConcurrentDictionary<string, string> store)
    {
        _store = store;
    }

    public Task SendPasswordResetEmailAsync(string email, string resetToken)
    {
        // Сохраняем токен по email, чтобы тест мог его забрать
        _store[email] = resetToken;
        return Task.CompletedTask;
    }

    // Если есть другие методы отправки, добавьте их сюда аналогично
}
