using CareerTracker.Identity.Data;
using CareerTracker.Identity.Domain;
using CareerTracker.Infrastructure.Auth;
using CareerTracker.Kernel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CareerTracker.Identity.Features.ResetPassword;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly UserDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = TokenHasher.Hash(request.Token);

        var resetRequest = await _context.PasswordResetRequests
            .FirstOrDefaultAsync(r => r.TokenHash == tokenHash && !r.IsUsed && r.ExpiresAt > DateTime.UtcNow, cancellationToken);

        if (resetRequest is null)
            return Result.Failure("Токен недействителен или истек");

        var user = await _context.Users.FindAsync([resetRequest.UserId], cancellationToken);
        if (user is null || !user.IsActive)
            return Result.Failure("Пользователь не найден");

        // Обновляем пароль через доменный метод
        user.SetPasswordHash(_passwordHasher.HashPassword(user, request.NewPassword));

        // Помечаем токен как использованный (одноразовость)
        resetRequest.MarkAsUsed();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
