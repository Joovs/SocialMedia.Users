using SocialMedia.Users.Domain.Entities.UserEntity.Models.UpdateProfile;

namespace SocialMedia.Users.Domain.Entities.UserEntity.Repositories;

public interface IUserRepository
{
    public Task<UpdateProfileResponseModel> UpdateProfile (UpdateProfileModel request, CancellationToken cancellationToken);
}
