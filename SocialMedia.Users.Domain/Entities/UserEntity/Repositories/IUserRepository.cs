namespace SocialMedia.Users.Domain.Entities.UserEntity.Repositories;

public interface IUserRepository
{
    public Task<User> ExampleUpdateUser (int id, CancellationToken cancellationToken);
}
