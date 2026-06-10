using CareerTracker.CareerPath.Data;
using CareerTracker.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CareerTracker.CareerPath.Features.CompleteLesson;

public class CompleteLessonHandler : IRequestHandler<CompleteLessonCommand, Result<bool>>
{
    private readonly CareerPathDbContext _dbContext;

    public CompleteLessonHandler(CareerPathDbContext dbContext, ILogger<CompleteLessonHandler> logger)
    {
        _dbContext = dbContext;
    }
    
    public Task<Result<bool>> Handle(CompleteLessonCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
