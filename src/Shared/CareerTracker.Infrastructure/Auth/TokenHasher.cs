using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace CareerTracker.Infrastructure.Auth;

public static class TokenHasher
{
    private static byte[] SecretKey = default!;
    
    public static void Initialize(IConfiguration config)
    {
        SecretKey = Encoding.UTF8.GetBytes(
            config["Security:TokenHmacSecret"] ?? throw new InvalidOperationException("Missing token secret"));
    }    

    /// <summary>
    /// Вычисляет HMAC-SHA256 хеш токена.
    /// </summary>
    public static string Hash(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentException("Token cannot be null or empty", nameof(token));

        using var hmac = new HMACSHA256(SecretKey);
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hashBytes);
    }
}
