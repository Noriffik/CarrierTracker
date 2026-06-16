using CareerTracker.CareerPath.Data;
using CareerTracker.Kernel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CareerTracker.CareerPath.Features.GetGoals;

public class GetGoalsHandler : IRequestHandler<GetGoalsQuery, Result<List<GoalDto>>>
{
    private readonly CareerPathDbContext _context;
    public GetGoalsHandler(CareerPathDbContext context) => _context = context;

    public async Task<Result<List<GoalDto>>> Handle(GetGoalsQuery request, CancellationToken cancellationToken)
    {
        var goals = await _context.Goals
            .Where(g => g.UserId == request.UserId && g.Status != Domain.GoalStatus.Archived)
            .OrderByDescending(g => g.CreatedAt)
            .Select(g => new GoalDto(g.Id, g.Title, g.Status.ToString(), g.Deadline))
            .ToListAsync(cancellationToken);

        return Result<List<GoalDto>>.Success(goals);
    }
}
