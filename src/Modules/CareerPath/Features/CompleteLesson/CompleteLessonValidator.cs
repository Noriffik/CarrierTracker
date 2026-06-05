using FluentValidation;

namespace CareerTracker.CareerPath.Features.CompleteLesson;

public class CompleteLessonValidator : AbstractValidator<CompleteLessonCommand>
{
    public CompleteLessonValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.LessonId).GreaterThan(0);
    }
}
