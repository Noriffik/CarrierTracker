using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.Delete;

public record DeleteUserCommand(int UserId) : IRequest<Result>;
