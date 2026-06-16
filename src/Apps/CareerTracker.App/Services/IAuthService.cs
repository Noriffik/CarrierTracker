using CareerTracker.App.Models;
using CareerTracker.Core.Domain.Models;
using System.Security.Cryptography;
using System.Text;

namespace CareerTracker.App.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginModel loginModel);
    Task<bool> RegisterAsync(RegisterModel registerModel);
    Task LogoutAsync();
    bool IsAuthenticated { get; }
    User? CurrentUser { get; }
    event Action? AuthStateChanged;
}

public class AuthService : IAuthService
{
    private User? _currentUser;

    private readonly Dictionary<string, User> _usersDatabase = new(); // Mock DB

    public bool IsAuthenticated => _currentUser != null;
    public User? CurrentUser => _currentUser;

    public event Action? AuthStateChanged;

    public AuthService()
    {
        // Добавляем тестового пользователя
        var testUser = new User
        {
            Id = 1,
            Email = "test@example.com",
            Password = HashPassword("password123"),
            FirstName = "Test",
            LastName = "User"
        };
        _usersDatabase[testUser.Email] = testUser;
    }

    public async Task<bool> LoginAsync(LoginModel loginModel)
    {
        await Task.Delay(100);

        if (_usersDatabase.TryGetValue(loginModel.Email, out var user))
        {
            if (VerifyPassword(loginModel.Password, user.Password))
            {
                _currentUser = new User
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                AuthStateChanged?.Invoke();
                return true;
            }
        }

        return false;
    }

    public async Task<bool> RegisterAsync(RegisterModel registerModel)
    {
        await Task.Delay(100);

        if (_usersDatabase.ContainsKey(registerModel.Email))
        {
            return false;
        }

        var user = new User
        {
            Id = _usersDatabase.Count + 1,
            Email = registerModel.Email,
            Password = HashPassword(registerModel.Password),
            FirstName = registerModel.FirstName,
            LastName = registerModel.LastName
        };

        _usersDatabase[registerModel.Email] = user;
        _currentUser = user;

        AuthStateChanged?.Invoke();
        return true;
    }

    public async Task LogoutAsync()
    {
        await Task.CompletedTask;
        _currentUser = null;
        AuthStateChanged?.Invoke();
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        var hashedInput = HashPassword(password);
        return hashedInput == hash;
    }
}
