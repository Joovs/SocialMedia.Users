namespace SocialMedia.Users.Application.DTO;
public class PostDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}