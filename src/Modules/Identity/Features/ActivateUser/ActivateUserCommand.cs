using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.ActivateUser;

public record ActivateUserCommand(int UserId) : IRequest<Result>;
