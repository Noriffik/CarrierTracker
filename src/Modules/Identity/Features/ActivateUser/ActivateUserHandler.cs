using CareerTracker.Identity.Data;
using CareerTracker.Identity.Domain;
using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.ActivateUser;

public class ActivateUserHandler : IRequestHandler<ActivateUserCommand, Result>
{
    private readonly UserDbContext _context;
    public ActivateUserHandler(UserDbContext context) => _context = context;

    public async Task<Result> Handle(ActivateUserCommand request, CancellationToken ct)
    {
        var user = await _context.Users.FindAsync(new object[] { request.UserId }, ct);
        if (user is null) return Result.Failure(ValidationErrors.UserNotFound);

        user.Activate();
        await _context.SaveChangesAsync(ct);
        return Result.Success();
    }
}