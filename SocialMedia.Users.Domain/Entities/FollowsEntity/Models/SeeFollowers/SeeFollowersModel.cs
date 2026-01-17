namespace SocialMedia.Users.Domain.Entities.FollowsEntity.Models.SeeFollowers;

public class SeeFollowersModel
{
    public required List<Follower> Followers { get; set; }
}

public class Follower
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
}