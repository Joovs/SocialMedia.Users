using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities.UserEntity.Repositories;
using SocialMedia.Users.Infrastructure.Persistence.Context;

namespace SocialMedia.Users.Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception($"User could not be created: {ex.Message}", ex);
        }
    }

    public async Task<User> ExampleUpdateUser(int id, CancellationToken cancellationToken)
    {
        try
        {
            User user = await (from us in _context.Users
                               where us.Id == id
                               select new User
                               {
                                   Id = us.Id,
                                   Username = us.Username,
                                   FistName = us.FistName,
                                   Lastname = us.Lastname,
                                   Email = us.Email,
                                   Password = us.Password,
                                   CreatedAt = us.CreatedAt,
                                   UpdatedAt = us.UpdatedAt,
                               }).FirstAsync(cancellationToken);

            User updatedUser = new User
            {
                Id = user.Id,
                Username = "New user",
                FistName = user.FistName,
                Lastname = user.Lastname,
                Email = user.Email,
                Password = user.Password,
                CreatedAt = user.CreatedAt,
                UpdatedAt = DateTime.Now
            };

            _context.Users.Update(updatedUser);
            await _context.SaveChangesAsync(cancellationToken);

            return updatedUser;
        }
        catch (Exception ex)
        {
            throw new Exception($"User could not be updated: {ex.Message}", ex);
        }
    }
}
