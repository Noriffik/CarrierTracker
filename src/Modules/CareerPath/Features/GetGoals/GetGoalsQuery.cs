using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.CareerPath.Features.GetGoals;

public record GetGoalsQuery(int UserId) : IRequest<Result<List<GoalDto>>>;

public record GoalDto(int Id, string Title, string Status, DateTime? Deadline);
