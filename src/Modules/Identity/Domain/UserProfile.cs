namespace CareerTracker.Identity.Domain;

public sealed class UserProfile
{
    public int Id { get; private set; }
    public int UserId { get; private set; } // FK к User

    // Основные поля профиля
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string? PhoneNumber { get; private set; }
    public string? City { get; private set; }

    // Гибкие атрибуты через JSONB (специфичные данные для разных ролей)
    // Выпускник: { "orphanage": "...", "graduationYear": 2020 }
    // Ветеран: { "unit": "...", "dischargeDate": "2023-05-01" }
    // Ментор: { "company": "...", "expertise": ["C#", ".NET"] }
    public Dictionary<string, object> Attributes { get; private set; } = new();

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Factory method
    public static UserProfile Create(int userId, string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("Имя обязательно");
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Фамилия обязательна");

        return new UserProfile
        {
            UserId = userId,
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateName(string firstName, string lastName)
    {
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateContact(string? phone, string? city)
    {
        PhoneNumber = phone?.Trim();
        City = city?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAttribute(string key, object value)
    {
        Attributes[key] = value;
        UpdatedAt = DateTime.UtcNow;
    }

    public T? GetAttribute<T>(string key)
    {
        return Attributes.TryGetValue(key, out var val) && val is T typed
            ? typed
            : default;
    }
}
