using SocialMedia.Users.Application.Queries.GetUserFollowing;

namespace SocialMedia.Users.Application.Repositories;

public interface IFollowRepository
{
    Task<List<UserFollowingDto>> GetFollowingAsync(Guid userId);
}
