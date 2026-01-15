using SocialMedia.Users.Domain.Entities;

namespace SocialMedia.Users.Application.Commands.Follow
{
    public class FollowUserCommand
    {
        public Guid FollowerUserId { get; set; }
        public Guid FollowingUserId { get; set; }
    }
}