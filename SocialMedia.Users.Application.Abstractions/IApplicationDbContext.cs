using System.Threading;
using System.Threading.Tasks;

namespace SocialMedia.Users.Application.Abstractions;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
