using CareerTracker.Identity.Data;
using CareerTracker.Identity.Domain;
using CareerTracker.Kernel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CareerTracker.Identity.Features.RegisterUser;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<int>>
{
    private readonly UserDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public RegisterUserHandler(UserDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<int>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Проверка уникальности email (можно вынести в UniqueConstraint БД, но дублируем здесь для быстрого фидбека)
        if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            return Result<int>.Failure("Email уже зарегистрирован");

        var user = User.Create(request.Email, request.Role);
        user.SetPasswordHash(_passwordHasher.HashPassword(user, request.Password));

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(user.Id);
    }
}