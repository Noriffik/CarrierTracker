using CareerTracker.Kernel.Common;

namespace CareerTracker.Identity.Domain;

public sealed class User
{
    public int Id { get; private set; }
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private ITimeProvider _timeProvider;

    private User()
    {
        _timeProvider = null!;
    }
    // Factory method для безопасного создания
    public static User Create(string email, UserRole role, ITimeProvider? timeProvider = null)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email не может быть пустым");

        return new User
        {
            Email = email.ToLowerInvariant(),
            Role = role,
            IsActive = true,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            _timeProvider = timeProvider ?? new SystemTimeProvider()
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
        if(!IsActive) return;
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if(IsActive) return;
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        if (IsDeleted) return;
        IsDeleted = true;
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
