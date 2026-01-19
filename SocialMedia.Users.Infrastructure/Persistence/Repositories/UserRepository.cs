using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities.UserEntity.Repositories;
using SocialMedia.Users.Infrastructure.Persistence.Context;

namespace SocialMedia.Users.Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;

}
