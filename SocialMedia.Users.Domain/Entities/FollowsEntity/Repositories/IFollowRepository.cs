using SocialMedia.Users.Domain.Entities.FollowsEntity.Models.SeeFollowers;

namespace SocialMedia.Users.Domain.Entities.FollowsEntity.Repositories;

public interface IFollowRepository
{
    public Task<bool> UserExists(Guid id, CancellationToken cancellationToken);
    public Task<List<Follower>> SeeFollowers(Guid id, CancellationToken cancellationToken);
}
