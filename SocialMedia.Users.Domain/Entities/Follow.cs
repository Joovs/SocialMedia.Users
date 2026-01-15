namespace SocialMedia.Users.Domain.Entities
{
    public class Follow
    {
        public Guid FollowerUserId { get; set; }
        public Guid FollowingUserId { get; set; }
        public bool IsActive { get; set; }
    }
}