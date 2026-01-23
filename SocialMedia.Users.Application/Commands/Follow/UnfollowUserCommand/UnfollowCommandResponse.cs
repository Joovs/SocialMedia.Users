namespace SocialMedia.Users.Application.Commands.Follow.UnfollowUserCommand;

public class UnfollowCommandResponse
{
    public Guid FollowerUserId { get; set; }
    public Guid FollowingUserId { get; set; }
    public string Status { get; set; } = string.Empty;
}
