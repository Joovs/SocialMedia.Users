using MediatR;

namespace SocialMedia.Users.Application.Commands.Follow.UnfollowUserCommand;

public class UnfollowUserCommand : IRequest<UnfollowCommandResponse>
{
    public Guid FollowerUserId { get; set; }
    public Guid FollowingUserId { get; set; }
}
