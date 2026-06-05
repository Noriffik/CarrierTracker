using CareerTracker.Identity.Data;
using CareerTracker.Identity.Domain;
using CareerTracker.Kernel;
using CareerTracker.Kernel.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CareerTracker.Identity.Features.Login;

public class LoginHandler : IRequestHandler<LoginCommand, Result<AuthTokensDto>>
{
    private readonly UserDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenService _jwtService; // Абстракция для генерации токенов

    public async Task<Result<AuthTokensDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(
            u => u.Email == request.Email.ToLowerInvariant(), cancellationToken);

        // Защита от тайминг-атак: всегда проверяем хэш, даже если user == null
        var dummyUser = new User();
        var verificationResult = user is not null
            ? _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password)
            : _passwordHasher.VerifyHashedPassword(dummyUser, "dummy", request.Password);

        if (user is null || verificationResult != PasswordVerificationResult.Success || !user.IsActive)
        {
            return Result<AuthTokensDto>.Failure("Неверный email или пароль");
        }

        var roleClaim = user.Role.ToString(); // "Graduate", "Veteran", "Mentor", "Admin"

        var accessToken = _jwtService.GenerateAccessToken(
            userId: user.Id,
            roleClaim: roleClaim,       // Передаем строку, а не enum
            lifetime: TimeSpan.FromMinutes(15)
        );

        var refreshToken = _jwtService.GenerateRefreshToken(user.Id);

        return Result<AuthTokensDto>.Success(new AuthTokensDto(AccessToken: accessToken,
            RefreshToken: refreshToken,
            ExpiresIn: 900
        ));

        // TODO: Сохранить refresh token hash в БД для возможности отзыва
        // await _context.RefreshTokens.AddAsync(new RefreshToken { ... });
        // await _context.SaveChangesAsync(ct);

        //return Result<AuthTokensDto>.Success(tokens);
    }
}
