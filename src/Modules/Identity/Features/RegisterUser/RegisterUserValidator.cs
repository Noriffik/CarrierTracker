using FluentValidation;

namespace CareerTracker.Identity.Features.RegisterUser;

public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email).EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).MinimumLength(8).WithMessage("Пароль должен содержать минимум 8 символов");
        RuleFor(x => x.Role).IsInEnum();
    }
}
