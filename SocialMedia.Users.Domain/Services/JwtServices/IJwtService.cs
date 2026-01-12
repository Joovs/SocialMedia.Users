namespace SocialMedia.Users.Domain.Services.JwtServices;

public interface IJwtService
{
    string GenerateToken(string email, int userId);
}