using SocialMedia.Users.Application.Queries.GetUserFollowing;
using SocialMedia.Users.Domain.Entities.FollowEntity;

namespace SocialMedia.Users.Application.Repositories;

public interface IFollowRepository
{
    Task<List<UserFollow>> GetFollowingAsync(Guid userId, CancellationToken cancellationToken);
}
