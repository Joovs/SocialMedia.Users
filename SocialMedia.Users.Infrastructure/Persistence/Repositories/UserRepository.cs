using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities.UserEntity.Repositories;
using SocialMedia.Users.Infrastructure.Persistence.Context;

namespace SocialMedia.Users.Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
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
}
