using CareerTracker.Identity.Domain;
using CareerTracker.Identity.Features.RegisterUser;
using FluentValidation.TestHelper;
using Xunit;

namespace CareerTracker.UnitTests.Identity.Features;

public class RegisterUserValidatorTests
{
    private readonly RegisterUserValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_HasNoErrors()
    {
        var command = new RegisterUserCommand("user@test.com", "StrongPass1!", UserRole.Graduate);
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    // Правильный подход: отдельный тест для каждого типа ошибки
    [Theory]
    [InlineData("invalid-email", "StrongPass1!", UserRole.Graduate, "Email")]
    [InlineData("user@test.com", "short", UserRole.Graduate, "Password")]
    [InlineData("user@test.com", "StrongPass1!", UserRole.Admin, "Role")]
    public void Validate_InvalidCommand_HasExpectedError(
        string email,
        string password,
        UserRole role,
        string expectedPropertyName)
    {
        var command = new RegisterUserCommand(email, password, role);
        var result = _validator.TestValidate(command);

        // Проверяем наличие ошибки у конкретного свойства
        result.ShouldHaveValidationErrorFor(GetPropertyExpression(expectedPropertyName));
    }

    // Вспомогательный метод для получения Expression по имени свойства
    private static System.Linq.Expressions.Expression<Func<RegisterUserCommand, object?>> GetPropertyExpression(string propertyName)
    {
        return propertyName switch
        {
            "Email" => x => x.Email,
            "Password" => x => x.Password,
            "Role" => x => x.Role,
            _ => throw new ArgumentException($"Unknown property: {propertyName}")
        };
    }
}