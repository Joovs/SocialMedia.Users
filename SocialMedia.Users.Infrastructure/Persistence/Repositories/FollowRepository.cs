using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Domain.Entities.FollowsEntity.Models.SeeFollowers;
using SocialMedia.Users.Infrastructure.Persistence.Context;

namespace SocialMedia.Users.Infrastructure.Persistence.Repositories;

public class FollowRepository : IFollowRepository
{
    private readonly ApplicationDbContext _context;

    public FollowRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UserExists(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users.AsNoTracking().AnyAsync(u => u.Id == id);
    }

    public async Task<List<Follower>> SeeFollowers(Guid id, CancellationToken cancellationToken)
    {
        List<Follower> followers = await (from u in _context.Users.AsNoTracking()
                                             join f in _context.Follows
                                             on u.Id equals f.FollowerId
                                             where f.FollowingId == id
                                             select new Follower
                                             {
                                                 Id = u.Id,
                                                 Username = u.Username
                                             }).ToListAsync();

        return followers;

    }
}
