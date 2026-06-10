using CareerTracker.Identity.Data;
using CareerTracker.Identity.Domain;
using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.DeactivateUser;

public class DeactivateUserHandler : IRequestHandler<DeactivateUserCommand, Result>
{
    private readonly UserDbContext _context;
    public DeactivateUserHandler(UserDbContext context) => _context = context;

    public async Task<Result> Handle(DeactivateUserCommand request, CancellationToken ct)
    {
        var user = await _context.Users.FindAsync(new object[] { request.UserId }, ct);
        if (user is null) return Result.Failure(ValidationErrors.UserNotFound);

        user.Deactivate();
        await _context.SaveChangesAsync(ct);
        return Result.Success();
    }
}
