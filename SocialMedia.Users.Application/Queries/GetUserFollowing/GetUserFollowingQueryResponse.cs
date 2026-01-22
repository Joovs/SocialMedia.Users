using SocialMedia.Users.Application.Queries.GetUserFollowing;

namespace SocialMedia.Users.Application.Queries.GetUserFollowing;

public class GetUserFollowingResponse
{
    public List<UserFollowingDto> Following { get; set; } = new();
}
