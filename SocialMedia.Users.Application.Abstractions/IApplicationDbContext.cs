using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Domain.Entities;
using SocialMedia.Users.Domain.Entities.UserEntity;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMedia.Users.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Follow> Follows { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
