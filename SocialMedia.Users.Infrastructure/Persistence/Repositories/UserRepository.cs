using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities.UserEntity.Models.UpdateProfile;
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
    public async Task<bool> UserExists(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users.AsNoTracking().AnyAsync(u => u.Id == id);
    }

    public async Task<UpdateProfileResponseModel> UpdateProfile(UpdateProfileModel request, CancellationToken cancellationToken)
    {
        User user = await _context.Users.FirstAsync(x => x.Id == request.Id, cancellationToken);

        bool exists = await _context.Users
        .AnyAsync(u => u.Email == request.Email && u.Id != request.Id, cancellationToken);

        if (exists)
        {
            throw new DuplicateEmailException(request.Email);
        }

        user.Username = request.Username;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.Password = request.Password;
        user.UpdateAt = DateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateProfileResponseModel
        {
            Id = request.Id,
            Username = user.Username,
            FirstName = request.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password,
            UpdatedAt = user.UpdateAt
        };
    }
}
