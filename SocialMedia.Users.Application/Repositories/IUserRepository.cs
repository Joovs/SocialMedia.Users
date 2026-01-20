using SocialMedia.Users.Domain.Entities.UserEntity;

namespace SocialMedia.Users.Application.Repositories;

public interface IUserRepository
{

    public Task<User> ExampleUpdateUser(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid userId);
}
