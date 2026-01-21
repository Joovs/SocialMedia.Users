using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Domain.Entities;
using SocialMedia.Users.Domain.Entities.Models;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Infrastructure.Persistence.Context;

namespace SocialMedia.Users.Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    public async Task<User> ExampleUpdateUser(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            User user = await (from us in context.Users
                                      where us.Id == id
                                      select new User
                                      {
                                          Id = us.Id,
                                          Username = us.Username,
                                          Lastname = us.Lastname,
                                          CreatedAt = us.CreatedAt,
                                          UpdateAt = us.UpdateAt,
                                      }).FirstAsync(cancellationToken);

            User updatedUser = new User
            {
                Id = user.Id,
                Username = "New user",
                Lastname = user.Lastname,
                CreatedAt = user.CreatedAt,
                UpdateAt = DateTime.Now
            };

            _context.Users.Update(updatedUser);
            await _context.SaveChangesAsync(cancellationToken);

            return updatedUser;
        }
        catch (Exception ex)
        {
            throw new Exception($"User could not be updated: {ex.Message}");
        }
    }

    public async Task<UserProfile> SeeProfile(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            UserProfile? userProfile = await (from us in context.Users
                                              where us.Id == userId
                                              select new UserProfile
                                              {
                                                  Id = us.Id,
                                                  Username = us.Username,
                                                  Lastname = us.Lastname,
                                                  Password = us.Password,
                                                  CreatedAt = us.CreatedAt,
                                                  UpdateAt = us.UpdateAt,
                                                  Posted = (from po in context.Posts
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