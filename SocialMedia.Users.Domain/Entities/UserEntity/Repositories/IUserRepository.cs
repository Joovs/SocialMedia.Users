namespace SocialMedia.Users.Domain.Entities.UserEntity.Repositories;

public interface IUserRepository
{
    public Task<User> ExampleUpdateUser(Guid id, CancellationToken cancellationToken);
    public Task<UserProfile> SeeProfile(Guid userId, CancellationToken cancellationToken);
}
