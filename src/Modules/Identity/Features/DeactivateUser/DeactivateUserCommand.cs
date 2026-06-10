using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.DeactivateUser;

public record DeactivateUserCommand(int UserId) : IRequest<Result>;
