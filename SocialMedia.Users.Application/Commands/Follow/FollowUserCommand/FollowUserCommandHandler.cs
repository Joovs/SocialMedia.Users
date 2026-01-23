using MediatR;
using SocialMedia.Users.Application.Abstractions;
using SocialMedia.Users.Domain.Entities;

namespace SocialMedia.Users.Application.Commands.Follow.FollowUserCommand;

public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, FollowCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public FollowUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<FollowCommandResponse> Handle(FollowUserCommand command, CancellationToken cancellationToken)
    {
        if (command.FollowerUserId == command.FollowingUserId)
            throw new ArgumentException("You cannot follow yourself");

        Follow? follow = await _unitOfWork.Follows.FindAsync(
            command.FollowerUserId,
            command.FollowingUserId);

        if (follow != null && follow.IsActive)
            throw new ArgumentException("You are already following this user");

        if (follow == null)
        {
            follow = new Follow
            {
                FollowerUserId = command.FollowerUserId,
                FollowingUserId = command.FollowingUserId,
                IsActive = true
            };
            await _unitOfWork.Follows.AddAsync(follow);
        }
        else
        {
            follow.IsActive = true;
            _unitOfWork.Follows.Update(follow);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        FollowCommandResponse response = new FollowCommandResponse
        {
            FollowerUserId = follow.FollowerUserId,
            FollowingUserId = follow.FollowingUserId,
            Status = "Followed"
        };

        return response;
    }
}
