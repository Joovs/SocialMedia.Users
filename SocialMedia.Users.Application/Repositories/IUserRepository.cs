using SocialMedia.Users.Domain.Entities.UserEntity.Models.UpdateProfile;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities;

namespace SocialMedia.Users.Application.Repositories;

public interface IUserRepository
{
    public Task<bool> UserExists(Guid id, CancellationToken cancellationToken);
    public Task<UpdateProfileResponseModel> UpdateProfile(UpdateProfileModel request, CancellationToken cancellationToken);
    public Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);
    public Task<UserProfile> SeeProfile(Guid userId, CancellationToken cancellationToken);
}
