using MediatR;

namespace SocialMedia.Users.Application.Commands.Follow.FollowUserCommand;

public class FollowUserCommand : IRequest<FollowCommandResponse>
{
    public Guid FollowerUserId { get; set; }
    public Guid FollowingUserId { get; set; }
}
