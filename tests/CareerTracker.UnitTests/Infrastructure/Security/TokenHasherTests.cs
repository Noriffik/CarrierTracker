using CareerTracker.Identity.Infrastructure;

namespace CareerTracker.UnitTests.Infrastructure.Security;

public class TokenHasherTests
{
    [Fact]
    public void Hash_SameToken_ProducesDifferentHashes()
    {
        // Уникальная соль гарантирует разные хэши
        var hash1 = TokenHasher.Hash("test-token");
        var hash2 = TokenHasher.Hash("test-token");

        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Verify_ValidToken_ReturnsTrue()
    {
        var token = "my-secret-reset-token";
        var hash = TokenHasher.Hash(token);

        Assert.True(TokenHasher.Verify(token, hash));
    }

    [Fact]
    public void Verify_WrongToken_ReturnsFalse()
    {
        var hash = TokenHasher.Hash("correct-token");

        Assert.False(TokenHasher.Verify("wrong-token", hash));
    }

    [Fact]
    public void Verify_InvalidBase64_ReturnsFalse()
    {
        Assert.False(TokenHasher.Verify("token", "not-valid-base64!!!"));
    }

    [Fact]
    public void Verify_NullInputs_ReturnsFalse()
    {
        Assert.False(TokenHasher.Verify(null!, "hash"));
        Assert.False(TokenHasher.Verify("token", null!));
    }
}