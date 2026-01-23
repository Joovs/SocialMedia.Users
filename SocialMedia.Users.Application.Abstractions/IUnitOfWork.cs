using SocialMedia.Users.Domain.Entities;
using SocialMedia.Users.Domain.Entities.UserEntity;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMedia.Users.Application.Abstractions;

public interface IUnitOfWork
{
    IRepository<User> Users { get; }
    IRepository<Follow> Follows { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
