using SocialMedia.Users.Domain.Entities.UserEntity;

namespace SocialMedia.Users.Application.Repositories;
public interface IUserRepository
{
    public Task<User> ExampleUpdateUser (Guid id, CancellationToken cancellationToken);
    public Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);
}