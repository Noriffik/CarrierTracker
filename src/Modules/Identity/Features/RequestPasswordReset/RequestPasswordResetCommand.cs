using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.RequestPasswordReset;

public record RequestPasswordResetCommand(string Email) : IRequest<Result>;
