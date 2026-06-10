using CareerTracker.Identity.Data;
using CareerTracker.Identity.Domain;
using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.Identity.Features.Delete;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly UserDbContext _context;
    public DeleteUserHandler(UserDbContext context) => _context = context;

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        var user = await _context.Users.FindAsync(new object[] { request.UserId }, ct);
        if (user is null) return Result.Failure(ValidationErrors.UserNotFound);

        user.SoftDelete();
        await _context.SaveChangesAsync(ct);
        return Result.Success();
    }
}