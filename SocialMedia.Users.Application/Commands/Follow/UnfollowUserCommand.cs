

namespace SocialMedia.Users.Application.Commands.Follow
{
    public class UnfollowUserCommand
    {
        public Guid FollowerUserId { get; set; }
        public Guid FollowingUserId { get; set; }
    }
}