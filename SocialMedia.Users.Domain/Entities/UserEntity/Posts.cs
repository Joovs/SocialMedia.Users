namespace SocialMedia.Users.Domain.Entities.UserEntity;
public class Posts
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; }
}