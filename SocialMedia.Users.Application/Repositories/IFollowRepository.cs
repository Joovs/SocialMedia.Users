using SocialMedia.Users.Domain.Entities.FollowsEntity.Models.SeeFollowers;

namespace SocialMedia.Users.Application.Repositories;

public interface IFollowRepository
{
    public Task<List<Follower>> SeeFollowers(Guid id, CancellationToken cancellationToken);
}
