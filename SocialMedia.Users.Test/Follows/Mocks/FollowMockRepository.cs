using SocialMedia.Users.Domain.Entities.FollowsEntity.Models.SeeFollowers;
using SocialMedia.Users.Application.Repositories;

namespace SocialMedia.Users.Test.Follows.Mocks;

public class FollowMockRepository : IFollowRepository, IUserRepository
{
    private readonly List<Guid> _database = new()
    {
        new Guid("6FBB9F92-3FD1-4C8A-747A-08DE53C6BA21"),
        new Guid("6FBB9F92-3FD1-4C8A-747A-08DE53C6BA22"),
        new Guid("6FBB9F92-3FD1-4C8A-747A-08DE53C6BA23")
    };

    public Task<List<Follower>> SeeFollowers(Guid id, CancellationToken cancellationToken)
    {
        List<Follower> response = new List<Follower>()
        {
            new Follower
            {
                Id = new Guid("6FBB9F92-3FD1-4C8A-747A-08DE53C6BA24"),
                Username = "Username4"
            },
            new Follower
            {
                Id = new Guid("6FBB9F92-3FD1-4C8A-747A-08DE53C6BA24"),
                Username = "Username4"
            }
        };

            return Task.FromResult(response);
    }

    public Task<bool> UserExists(Guid id, CancellationToken cancellationToken)
    {
        bool exists = _database.Any(u =>
            u == id);

        if (!exists)
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }
}
