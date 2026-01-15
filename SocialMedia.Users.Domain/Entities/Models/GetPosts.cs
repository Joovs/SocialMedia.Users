namespace SocialMedia.Users.Domain.Entities.Models;
public class GetPosts
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; }
}