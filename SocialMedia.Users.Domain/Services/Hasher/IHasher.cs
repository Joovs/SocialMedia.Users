namespace SocialMedia.Users.Domain.Services.Hasher;

public interface IHasher
{
    string hashPassword(string password);
}
