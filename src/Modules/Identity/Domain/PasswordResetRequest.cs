namespace CareerTracker.Identity.Domain;

/// <summary>
/// Агрегат запроса на сброс пароля.
/// Хранит ТОЛЬКО хэш токена — сам токен никогда не сохраняется в БД.
/// </summary>
public sealed class PasswordResetRequest
{
    // EF Core требует публичный конструктор без параметров для проксирования
    private PasswordResetRequest() { }

    public int Id { get; private set; }

    /// <summary>
    /// Внешний ключ к пользователю.
    /// Навигационное свойство НЕ включено, чтобы избежать лишних JOIN при поиске по токену.
    /// </summary>
    public int UserId { get; private set; }

    /// <summary>
    /// HMAC-SHA256 хэш токена сброса.
    /// Максимальная длина 64 символа (Base64 от 32-байтового SHA256).
    /// </summary>
    public string TokenHash { get; private set; } = null!;

    /// <summary>
    /// Время истечения срока действия токена.
    /// Стандартный TTL: 1 час.
    /// </summary>
    public DateTime ExpiresAt { get; private set; }

    /// <summary>
    /// Флаг одноразового использования.
    /// После успешного сброса пароля устанавливается в true и больше не меняется.
    /// </summary>
    public bool IsUsed { get; private set; }

    /// <summary>
    /// Время создания запроса (для аудита и аналитики).
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Factory method для безопасного создания записи.
    /// Гарантирует, что все инварианты соблюдены при создании.
    /// </summary>
    /// <param name="userId">ID пользователя</param>
    /// <param name="tokenHash">Предварительно вычисленный хэш токена</param>
    /// <param name="lifetime">Срок действия токена (рекомендуется TimeSpan.FromHours(1))</param>
    /// <exception cref="ArgumentException">Если tokenHash пуст или lifetime отрицательный</exception>
    public static PasswordResetRequest Create(int userId, string tokenHash, TimeSpan lifetime)
    {
        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new ArgumentException("Token hash cannot be empty", nameof(tokenHash));

        if (lifetime <= TimeSpan.Zero)
            throw new ArgumentException("Lifetime must be positive", nameof(lifetime));

        return new PasswordResetRequest
        {
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.Add(lifetime),
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Помечает запрос как использованный.
    /// Метод идемпотентен: повторный вызов не меняет состояние.
    /// </summary>
    /// <returns>True, если состояние изменилось; False, если уже был использован</returns>
    public bool MarkAsUsed()
    {
        if (IsUsed) return false;

        IsUsed = true;
        return true;
    }

    /// <summary>
    /// Проверяет, действителен ли запрос прямо сейчас.
    /// Используется в хендлерах для быстрой проверки перед обращением к БД.
    /// </summary>
    public bool IsValid => !IsUsed && ExpiresAt > DateTime.UtcNow;
}