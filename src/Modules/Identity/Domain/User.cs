namespace CareerTracker.Identity.Domain;

public sealed class User
{
    public int Id { get; private set; }
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Factory method для безопасного создания
    public static User Create(string email, UserRole role)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email не может быть пустым");

        return new User
        {
            Email = email.ToLowerInvariant(),
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void SetPasswordHash(string hash) => PasswordHash = hash;
    public void ChangeRole(UserRole newRole)
    {
        if (Role == newRole) return;
        Role = newRole;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
