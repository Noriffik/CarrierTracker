using FluentValidation;

namespace CareerTracker.Identity.Features.UpdateUserProfile;

public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.FirstName).MaximumLength(100).NotEmpty();
        RuleFor(x => x.LastName).MaximumLength(100).NotEmpty();
        RuleFor(x => x.PhoneNumber).MaximumLength(20).Matches(@"^\+?[0-9\s\-()]+$").When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}
