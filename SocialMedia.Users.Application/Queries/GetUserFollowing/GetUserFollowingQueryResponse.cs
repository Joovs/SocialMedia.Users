using SocialMedia.Users.Application.Queries.GetUserFollowing;
using SocialMedia.Users.Domain.Entities.FollowEntity;

namespace SocialMedia.Users.Application.Queries.GetUserFollowing;

public class GetUserFollowingQueryResponse
{
    public List<UserFollow> Following { get; set; } = new();
}
