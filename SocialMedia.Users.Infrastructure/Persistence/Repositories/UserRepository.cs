using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities.UserEntity.Models.UpdateProfile;
using SocialMedia.Users.Domain.Entities.UserEntity.Repositories;
using SocialMedia.Users.Domain.Exceptions;
using SocialMedia.Users.Infrastructure.Persistence.Context;

namespace SocialMedia.Users.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateProfileResponseModel> UpdateProfile(UpdateProfileModel request, CancellationToken cancellationToken)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(); 
        }

        bool exists = await _context.Users
        .AnyAsync(u => u.Email == request.Email && u.Id != request.Id, cancellationToken);

        if (exists)
        {
            throw new DuplicateEmailException(request.Email);
        }

        user.Username = request.Username;
        user.Lastname = request.Lastname;
        user.Email = request.Email;
        user.Password = request.Password;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateProfileResponseModel
        {
            Message = "Profile successfully updated",
            UpdatedAt = user.UpdatedAt
        };
    }
}
