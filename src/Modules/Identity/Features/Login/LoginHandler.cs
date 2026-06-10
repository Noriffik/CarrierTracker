using CareerTracker.Identity.Data;
using CareerTracker.Identity.Domain;
using CareerTracker.Kernel;
using CareerTracker.Kernel.Common;
using CareerTracker.Kernel.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CareerTracker.Identity.Features.Login;

public class LoginHandler : IRequestHandler<LoginCommand, Result<AuthTokensDto>>
{
    private readonly UserDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenService _jwtService;

    public LoginHandler(UserDbContext context, IPasswordHasher<User> passwordHasher, IJwtTokenService jwtService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthTokensDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(
            u => u.Email == request.Email.ToLowerInvariant(), cancellationToken);

        var dummyUser = User.Create(user.Email, user.Role, null);
        var verificationResult = user is not null
            ? _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password)
            : _passwordHasher.VerifyHashedPassword(dummyUser, "dummy", request.Password);

        if (user is null || verificationResult != PasswordVerificationResult.Success || !user.IsActive)
        {
            return Result<AuthTokensDto>.Failure(ValidationErrors.InvalidCredentials);
        }

        var accessToken = _jwtService.GenerateAccessToken(user.Id, user.Role.ToString(), TimeSpan.FromMinutes(15));
        var refreshToken = _jwtService.GenerateRefreshToken(user.Id);

        return Result<AuthTokensDto>.Success(new AuthTokensDto(accessToken, refreshToken, 900));
    }
}
