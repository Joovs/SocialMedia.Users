namespace SocialMedia.Users.Domain.Entities.FollowEntity;

public class UserFollow
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
}
