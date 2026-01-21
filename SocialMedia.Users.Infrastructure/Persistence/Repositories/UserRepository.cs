using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Domain.Entities.UserEntity;
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

}
