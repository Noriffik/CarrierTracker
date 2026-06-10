using CareerTracker.Identity.Domain;
using FluentValidation;

namespace CareerTracker.Identity.Features.ChangeUserRole;

public class ChangeUserRoleValidator : AbstractValidator<ChangeUserRoleCommand>
{
    public ChangeUserRoleValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.NewRole).IsInEnum();

        // 🛡️ Защита от эскалации привилегий: нельзя назначить Admin через этот эндпоинт
        RuleFor(x => x.NewRole).NotEqual(UserRole.Admin)
            .WithMessage(ValidationErrors.CannotAssignAdminRole);
    }
}
