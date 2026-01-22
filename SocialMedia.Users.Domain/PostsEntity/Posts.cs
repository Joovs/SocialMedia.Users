namespace SocialMedia.Users.Domain.PostsEntity;
public class Posts
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}