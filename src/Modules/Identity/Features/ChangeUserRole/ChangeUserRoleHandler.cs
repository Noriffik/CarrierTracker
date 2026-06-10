using CareerTracker.Identity.Data;
using CareerTracker.Identity.Domain;
using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.ChangeUserRole;

public class ChangeUserRoleHandler : IRequestHandler<ChangeUserRoleCommand, Result>
{
    private readonly UserDbContext _context;

    public ChangeUserRoleHandler(UserDbContext context) => _context = context;

    public async Task<Result> Handle(ChangeUserRoleCommand request, CancellationToken ct)
    {
        var user = await _context.Users.FindAsync(new object[] { request.UserId }, ct);
        if (user is null) return Result.Failure(ValidationErrors.UserNotFound);

        user.ChangeRole(request.NewRole);

        await _context.SaveChangesAsync(ct);

        // TODO: Senior tip - Здесь должно быть опубликовано доменное событие 
        // или запись в AuditLog: "Admin X changed role of User Y to Z"

        return Result.Success();
    }
}
