using CareerTracker.Identity.Data;
using CareerTracker.Identity.Domain;
using CareerTracker.Identity.Infrastructure;
using CareerTracker.Kernel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CareerTracker.Identity.Features.ResetPassword;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Result>
{
    private const string TokenExpiredOrUsed = "Ссылка для сброса пароля больше не действительна";
    private const string ResetRaceCondition = "Запрос обрабатывается параллельно. Повторите попытку.";

    private readonly UserDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public ResetPasswordHandler(UserDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = TokenHasher.Hash(request.Token);

        var resetRequest = await _context.PasswordResetRequests
            .FirstOrDefaultAsync(r => r.TokenHash == tokenHash && !r.IsUsed && r.ExpiresAt > DateTime.UtcNow, cancellationToken);

        if (resetRequest is null)
            return Result.Failure(TokenExpiredOrUsed);

        var user = await _context.Users.FindAsync([resetRequest.UserId], cancellationToken);
        if (user is null || !user.IsActive)
            return Result.Failure(ValidationErrors.AccountDeactivated);

        // Обновляем пароль через доменный метод
        user.SetPasswordHash(_passwordHasher.HashPassword(user, request.NewPassword));

        // Помечаем токен как использованный (одноразовость)
        resetRequest.MarkAsUsed();

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Failure(ResetRaceCondition);
        }

        return Result.Success();
    }
}
