using FluentValidation;

namespace CareerTracker.CareerPath.Features.CreateGoal;

public class CreateGoalValidator : AbstractValidator<CreateGoalCommand>
{
    public CreateGoalValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Deadline).GreaterThan(DateTime.UtcNow).When(x => x.Deadline.HasValue);
    }
}
