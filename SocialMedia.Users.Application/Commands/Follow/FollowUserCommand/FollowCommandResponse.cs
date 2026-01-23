namespace SocialMedia.Users.Application.Commands.Follow.FollowUserCommand;

public class FollowCommandResponse
{
    public Guid FollowerUserId { get; set; }
    public Guid FollowingUserId { get; set; }
    public string Status { get; set; } = string.Empty;
}
