namespace CareerTracker.Identity.Domain;

internal static class ValidationErrors
{
    // Auth
    public const string EmailAlreadyRegistered = "Этот email уже используется";
    public const string CannotRegisterAsAdmin = "Самостоятельная регистрация админом запрещена";
    public const string InvalidCredentials = "Неверный email или пароль";
    public const string AccountDeactivated = "Аккаунт деактивирован";

    // Profile
    public const string ProfileNotFound = "Профиль не найден";
    public const string FirstNameRequired = "Имя обязательно";
    public const string LastNameRequired = "Фамилия обязательна";

    // Password Reset
    public const string PasswordResetTokenInvalid = "Токен сброса недействителен или истек";

    public const string UserNotFound = "Пользователь не найден или деактивирован";
    public const string RoleAlreadyRevoked = "Пользователь уже с базовой ролью";
    public const string CannotAssignAdminRole = "Yельзя назначить роль администратора через этот эндпоинт";
}
