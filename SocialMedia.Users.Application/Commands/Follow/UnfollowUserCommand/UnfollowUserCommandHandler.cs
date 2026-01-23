using MediatR;
using SocialMedia.Users.Application.Abstractions;

namespace SocialMedia.Users.Application.Commands.Follow.UnfollowUserCommand;

public class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand, UnfollowCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UnfollowUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UnfollowCommandResponse> Handle(UnfollowUserCommand command, CancellationToken cancellationToken)
    {
        Follow? follow = await _unitOfWork.Follows.FindAsync(
            command.FollowerUserId,
            command.FollowingUserId);

        if (follow == null || !follow.IsActive)
            throw new ArgumentException("You are not following this user");

        follow.IsActive = false;
        _unitOfWork.Follows.Update(follow);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        UnfollowCommandResponse response = new UnfollowCommandResponse
        {
            FollowerUserId = follow.FollowerUserId,
            FollowingUserId = follow.FollowingUserId,
            Status = "Unfollowed"
        };

        return response;
    }
}

