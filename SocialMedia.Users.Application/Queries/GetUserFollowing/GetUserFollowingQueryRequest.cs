namespace SocialMedia.Users.Application.Queries.GetUserFollowing
{
    public class GetUserFollowingQueryRequest
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
