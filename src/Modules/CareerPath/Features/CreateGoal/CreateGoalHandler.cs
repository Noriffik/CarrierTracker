using CareerTracker.CareerPath.Data;
using CareerTracker.CareerPath.Domain;
using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.CareerPath.Features.CreateGoal;

public class CreateGoalHandler : IRequestHandler<CreateGoalCommand, Result<int>>
{
    private readonly CareerPathDbContext _context;

    public CreateGoalHandler(CareerPathDbContext context) => _context = context;

    public async Task<Result<int>> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = CareerGoal.Create(request.UserId, request.Title, request.Description, request.Deadline);
        try
        {
            _context.Goals.Add(goal);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(goal.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure(ex.Message);
        }
    }
}
