using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Application.Queries.GetUserFollowing;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Domain.Entities.FollowEntity;
using SocialMedia.Users.Infrastructure.Persistence.Context;

namespace SocialMedia.Users.Infrastructure.Persistence.Repositories;

public class FollowRepository : IFollowRepository
{
    private readonly ApplicationDbContext _context;

    public FollowRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserFollow>> GetFollowingAsync(Guid userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _context.Follows
            .Where(f => f.FollowerId == userId)
            .Join(
                _context.Users,
                f => f.FollowingId,
                u => u.Id,
                (f, u) => new UserFollow
                {
                    UserId = u.Id,
                    Username = u.Username
                }
            )
            .ToListAsync(cancellationToken);
    }

}
