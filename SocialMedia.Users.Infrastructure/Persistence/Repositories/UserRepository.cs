using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Domain.Entities;
using SocialMedia.Users.Domain.Entities.Models;
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

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
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

    public async Task<UserProfile> SeeProfile(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            UserProfile? userProfile = await (from us in _context.Users
                                              where us.Id == userId
                                              select new UserProfile
                                              {
                                                  Id = us.Id,
                                                  Username = us.Username,
                                                  Lastname = us.LastName,
                                                  Password = us.Password,
                                                  CreatedAt = us.CreatedAt,
                                                  UpdateAt = us.UpdateAt,
                                                  Posted = (from po in _context.Posts
                                                            where po.UserId == userId
                                                            select new GetPosts
                                                            {
                                                                Id = po.Id,
                                                                UserId = po.UserId,
                                                                Body = po.Body,
                                                                CreatedAt = po.CreatedAt
                                                            }).ToList()
                                              }).FirstOrDefaultAsync(cancellationToken);
            return userProfile;
        } catch (Exception ex)
        {
            throw new Exception($"Data could not be fetch: {ex.Message}");
        }
    }
}