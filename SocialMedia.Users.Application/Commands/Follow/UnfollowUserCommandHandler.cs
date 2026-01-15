using SocialMedia.Users.Domain.Entities;
using SocialMedia.Users.Application.Abstractions;

namespace SocialMedia.Users.Application.Commands.Follow
{
    public class UnfollowUserCommandHandler
    {
       private readonly IApplicationDbContext _context;

        public UnfollowUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FollowCommandResponse> Handle(UnfollowUserCommand command)
        {
            var follow = await _context.Follows.FindAsync(
                command.FollowerUserId,
                command.FollowingUserId);

            if (follow == null || !follow.IsActive)
                throw new ArgumentException("No sigues a este usuario");

            follow.IsActive = false;
            await _context.SaveChangesAsync();

            return new FollowCommandResponse
            {
                FollowerUserId = follow.FollowerUserId,
                FollowingUserId = follow.FollowingUserId,
                Status = "Unfollowed"
            };  
    }
    }   
}
