using SocialMedia.Users.Domain.Entities.UserEntity.Models.UpdateProfile;

namespace SocialMedia.Users.Application.Repositories;

public interface IUserRepository
{
    public Task<bool> UserExists(Guid id, CancellationToken cancellationToken);
    public Task<UpdateProfileResponseModel> UpdateProfile(UpdateProfileModel request, CancellationToken cancellationToken);
}
