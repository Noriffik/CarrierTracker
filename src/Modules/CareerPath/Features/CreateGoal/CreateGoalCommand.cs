using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.CareerPath.Features.CreateGoal;

public record CreateGoalCommand(int UserId, string Title, string Description, DateTime? Deadline) : IRequest<Result<int>>;
