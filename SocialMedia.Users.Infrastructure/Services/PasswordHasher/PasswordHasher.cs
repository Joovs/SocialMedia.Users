using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using SocialMedia.Users.Domain.Services.PasswordHashes;

namespace SocialMedia.Users.Infrastructure.Services.PasswordHasher;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<string> _identityPasswordHasher = new();

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword))
        {
            return false;
        }

        try
        {
            if (IsBCryptHash(hashedPassword))
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }

            if (IsSha256Hash(hashedPassword))
            {
                string sha256 = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password))).ToLowerInvariant();
                return string.Equals(sha256, hashedPassword, StringComparison.OrdinalIgnoreCase);
            }

            if (IsIdentityHash(hashedPassword))
            {
                PasswordVerificationResult identityResult = _identityPasswordHasher.VerifyHashedPassword("legacy", hashedPassword, password);
                return identityResult is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
            }

            // Fallback for legacy plain-text storage. TODO: remove once all passwords are hashed.
            return string.Equals(password, hashedPassword, StringComparison.Ordinal);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            return string.Equals(password, hashedPassword, StringComparison.Ordinal);
        }
    }

    private static bool IsBCryptHash(string hash)
    {
        return hash.StartsWith("$2a$") || hash.StartsWith("$2b$") || hash.StartsWith("$2y$");
    }

    private static bool IsSha256Hash(string hash)
    {
        if (hash.Length != 64)
        {
            return false;
        }

        foreach (char c in hash)
        {
            bool isHex = (c >= '0' && c <= '9')
                || (c >= 'a' && c <= 'f')
                || (c >= 'A' && c <= 'F');

            if (!isHex)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsIdentityHash(string hash)
    {
        return hash.StartsWith("AQAAAA", StringComparison.Ordinal);
    }
}