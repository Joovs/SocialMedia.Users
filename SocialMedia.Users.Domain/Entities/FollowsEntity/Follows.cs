namespace SocialMedia.Users.Domain.Entities.FollowsEntity;

public class Follows
{
    public Guid FollowerId { get; set; }
    public Guid FollowingId { get; set; }
    public DateTime CreatedAt { get; set; }
}
