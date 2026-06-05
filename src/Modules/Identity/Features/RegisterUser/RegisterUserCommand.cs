using CareerTracker.Identity.Domain;
using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.RegisterUser;

public sealed record RegisterUserCommand(string Email, string Password, UserRole Role) : IRequest<Result<int>>;
