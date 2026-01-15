using SocialMedia.Users.Application.DTO;

namespace SocialMedia.Users.Application.Users.Queries.SeeProfile;
public class SeeProfileQueryResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public List<PostDTO>? Posted { get; set; }

}