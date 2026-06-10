using CareerTracker.Identity.Data;
using CareerTracker.Identity.Domain;
using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.RevokeUserRole;

public class RevokeUserRoleHandler : IRequestHandler<RevokeUserRoleCommand, Result>
{
    private readonly UserDbContext _context;

    public RevokeUserRoleHandler(UserDbContext context) => _context = context;

    public async Task<Result> Handle(RevokeUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);
        if (user is null) return Result.Failure(ValidationErrors.UserNotFound);

        // Если у пользователя уже базовая роль, операция не имеет смысла
        if (user.Role == UserRole.Graduate)
        {
            return Result.Failure(ValidationErrors.RoleAlreadyRevoked);
        }

        // Сброс к базовой роли
        user.ChangeRole(UserRole.Graduate);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}