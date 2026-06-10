using System.Security.Cryptography;
using System.Text;

namespace CareerTracker.Identity.Infrastructure;

public static class TokenHasher
{
    private const string TokenIsNulOrEmpty = "Token cannot be null or empty";
    // Параметры PBKDF2 (соответствуют стандартам OWASP 2024)
    private const int SaltSize = 16;      // 128 бит
    private const int HashSize = 32;      // 256 бит
    private const int Iterations = 600_000; // Минимум для SHA256 в 2024 году

    // Формат хранения: [Salt(16 bytes)][Hash(32 bytes)] -> Base64
    private const int TotalBytes = SaltSize + HashSize;


    /// <summary>
    /// Вычисляет HMAC-SHA256 хеш токена.
    /// </summary>
    public static string Hash(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentException(TokenIsNulOrEmpty, nameof(token));

        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(token),
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        byte[] result = new byte[TotalBytes];
        Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, result, SaltSize, HashSize);

        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// Проверяет токен against stored hash.
    /// Использует constant-time comparison для защиты от timing attacks.
    /// </summary>
    public static bool Verify(string token, string storedHash)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(storedHash))
            return false;

        try
        {
            byte[] decoded = Convert.FromBase64String(storedHash);
            if (decoded.Length != TotalBytes) return false;

            byte[] salt = new byte[SaltSize];
            byte[] expectedHash = new byte[HashSize];

            Buffer.BlockCopy(decoded, 0, salt, 0, SaltSize);
            Buffer.BlockCopy(decoded, SaltSize, expectedHash, 0, HashSize);

            byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(token),
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            // Constant-time comparison (защита от timing attack)
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch (FormatException)
        {
            return false; // Невалидный Base64
        }
    }
}
