using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.CareerPath.Features.CompleteLesson;

public sealed record CompleteLessonCommand(int UserId, int LessonId) : IRequest<Result<bool>>;
