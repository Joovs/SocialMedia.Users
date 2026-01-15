using SocialMedia.Users.Domain.Entities;
using SocialMedia.Users.Application.Abstractions;
namespace SocialMedia.Users.Application.Commands.Follow
{
    public class FollowUserCommandHandler
    {
        private readonly IApplicationDbContext _context;

        public FollowUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FollowCommandResponse> Handle(FollowUserCommand command)
        {
            if (command.FollowerUserId == command.FollowingUserId)
                throw new ArgumentException("No puedes seguirte a ti mismo");

            var follow = await _context.Follows.FindAsync(
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
                _context.Follows.Add(follow);
            }
            else
            {
                follow.IsActive = true;
            }

            await _context.SaveChangesAsync();

            return new FollowCommandResponse
            {
                FollowerUserId = follow.FollowerUserId,
                FollowingUserId = follow.FollowingUserId,
                Status = "Followed"
            };
        }
    }
}