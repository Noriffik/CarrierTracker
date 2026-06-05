using CareerTracker.Kernel;
using MediatR;

namespace CareerTracker.CareerPath.Features.CompleteLesson;

public class CompleteLessonHandler : IRequestHandler<CompleteLessonCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(CompleteLessonCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
