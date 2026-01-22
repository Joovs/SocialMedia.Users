using SocialMedia.Users.Domain.Entities;
using SocialMedia.Users.Application.Abstractions;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Users.Application.Commands.Follow
{
    public class UnfollowUserCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnfollowUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FollowCommandResponse> Handle(UnfollowUserCommand command)
        {
            var follow = await _unitOfWork.Follows.FindAsync(
                command.FollowerUserId,
                command.FollowingUserId);

            if (follow == null || !follow.IsActive)
                throw new ArgumentException("No sigues a este usuario");

            follow.IsActive = false;
            _unitOfWork.Follows.Update(follow);
            await _unitOfWork.SaveChangesAsync();

            return new FollowCommandResponse
            {
                FollowerUserId = follow.FollowerUserId,
                FollowingUserId = follow.FollowingUserId,
                Status = "Unfollowed"
            };  
        }
    }
}
