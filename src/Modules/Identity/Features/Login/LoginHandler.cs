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
    private const string DummyPasswordHash = "AQAAAAIAAYagAAAAEP7l5qV8Y3K7lZ9wX2bN4mP5qR6sT7uV8wX9yZ0aB1cD2eF3gH4iJ5kL6mN7oP8qR9sT0uV1wX2yZ3aB4cD5eF6gH7iJ8kL9mN0oP1qR2sT3uV4wX5yZ6aB7cD8eF9gH0iJ1kL2mN3oP4qR5sT6uV7wX8yZ9aB0cD1eF2gH3iJ4kL5mN6oP7qR8sT9uV0wX1yZ2aB3cD4eF5gH6iJ7kL8mN9oP0qR1sT2uV3wX4yZ5aB6cD7eF8gH9iJ0kL1mN2oP3qR4sT5uV6wX7yZ8aB9cD0eF1gH2iJ3kL4mN5oP6qR7sT8uV9wX0yZ1aB2cD3eF4gH5iJ6kL7mN8oP9qR0sT1uV2wX3yZ4aB5cD6eF7gH8iJ9kL0mN1oP2qR3sT4uV5wX6yZ7aB8cD9eF0gH1iJ2kL3mN4oP5qR6sT7uV8wX9yZ0aB1cD2eF3gH4iJ5kL6mN7oP8qR9s==";

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
            : _passwordHasher.VerifyHashedPassword(dummyUser, DummyPasswordHash, request.Password);

        if (user is null || verificationResult != PasswordVerificationResult.Success || !user.IsActive)
        {
            return Result<AuthTokensDto>.Failure(ValidationErrors.InvalidCredentials);
        }

        var accessToken = _jwtService.GenerateAccessToken(user.Id, user.Role.ToString(), TimeSpan.FromMinutes(15));
        var refreshToken = _jwtService.GenerateRefreshToken(user.Id);

        return Result<AuthTokensDto>.Success(new AuthTokensDto(accessToken, refreshToken, 900));
    }
}
