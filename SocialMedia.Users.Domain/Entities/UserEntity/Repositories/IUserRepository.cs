using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMedia.Users.Domain.Entities.UserEntity.Repositories;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);
    Task<User> ExampleUpdateUser (Guid id, CancellationToken cancellationToken);
}
