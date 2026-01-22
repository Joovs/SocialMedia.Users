using SocialMedia.Users.Domain.Entities;
using SocialMedia.Users.Application.Abstractions;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Users.Application.Commands.Follow
{
    public class FollowUserCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public FollowUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FollowCommandResponse> Handle(FollowUserCommand command)
        {
            if (command.FollowerUserId == command.FollowingUserId)
                throw new ArgumentException("No puedes seguirte a ti mismo");

            var follow = await _unitOfWork.Follows.FindAsync(
                command.FollowerUserId,
                command.FollowingUserId);

            if (follow != null && follow.IsActive)
                throw new ArgumentException("Ya sigues a este usuario");

            if (follow == null)
            {
                follow = new global::SocialMedia.Users.Domain.Entities.Follow
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

            await _unitOfWork.SaveChangesAsync();

            return new FollowCommandResponse
            {
                FollowerUserId = follow.FollowerUserId,
                FollowingUserId = follow.FollowingUserId,
                Status = "Followed"
            };
        }
    }
}