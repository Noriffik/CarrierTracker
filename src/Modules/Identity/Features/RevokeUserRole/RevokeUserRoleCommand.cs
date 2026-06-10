using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.RevokeUserRole;

public record RevokeUserRoleCommand(int UserId) : IRequest<Result>;
