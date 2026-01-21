namespace SocialMedia.Users.Application.Repositories;

public interface IUserRepository
{
    public Task<bool> UserExists(Guid id, CancellationToken cancellationToken);
}
