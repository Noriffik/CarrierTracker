using CareerTracker.Identity.Domain;
using CareerTracker.Identity.Features.RegisterUser;
using FluentValidation.TestHelper;
using Microsoft.IdentityModel.Tokens.Experimental;
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

    [Fact]
    public void Validate_InvalidEmail_HasError()
    {
        var command = new RegisterUserCommand("invalid-email", "StrongPass1!", UserRole.Graduate);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_ShortPassword_HasError()
    {
        var command = new RegisterUserCommand("user@test.com", "short", UserRole.Graduate);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_AdminRole_HasError()
    {
        var command = new RegisterUserCommand("user@test.com", "StrongPass1!", UserRole.Admin);
        var result = _validator.TestValidate(command);

        // Проверяем, что ошибка есть именно у свойства Role
        result.ShouldHaveValidationErrorFor(x => x.Role);

        // Опционально: проверяем текст ошибки
        result.ShouldHaveValidationErrorFor(x => x.Role)
              .WithErrorMessage("Самостоятельная регистрация админом запрещена");
    }
}