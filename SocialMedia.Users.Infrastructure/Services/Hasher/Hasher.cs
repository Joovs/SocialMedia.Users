using Microsoft.Extensions.Configuration;
using SocialMedia.Users.Domain.Services.Hasher;

namespace SocialMedia.Users.Infrastructure.Services.Hasher;

public class Hasher(IConfiguration configuration) : IHasher
{
    private readonly string _prefix = configuration["Security:PasswordPrefix"];
    public string hashPassword(string password)
    {
        string passwordWithPrefix = _prefix + password;
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordWithPrefix);
        return hashedPassword;
    }
}
