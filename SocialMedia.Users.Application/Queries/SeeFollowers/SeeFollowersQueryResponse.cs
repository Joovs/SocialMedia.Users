using SocialMedia.Users.Domain.Entities.FollowsEntity.Models.SeeFollowers;

namespace SocialMedia.Users.Application.Queries.SeeFollowers;

public class SeeFollowersQueryResponse
{
    public Guid UserId { get; set; }
    public required List<Follower> Followers { get; set; }
}
