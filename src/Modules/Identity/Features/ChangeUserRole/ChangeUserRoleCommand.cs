using CareerTracker.Identity.Domain;
using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.ChangeUserRole;

public record ChangeUserRoleCommand(int UserId, UserRole NewRole) : IRequest<Result>;
