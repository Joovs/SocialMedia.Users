using SocialMedia.Users.Domain.Entities;
using SocialMedia.Users.Domain.Entities.UserEntity;

namespace SocialMedia.Users.Application.Repositories;
public interface IUserRepository
{
    public Task<User> ExampleUpdateUser(Guid id, CancellationToken cancellationToken);
    public Task<UserProfile> SeeProfile(Guid userId, CancellationToken cancellationToken);
}