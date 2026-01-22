using SocialMedia.Users.Domain.Entities;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Application.Abstractions;
using SocialMedia.Users.Infrastructure.Persistence.Context;
using SocialMedia.Users.Infrastructure.Persistence.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMedia.Users.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IRepository<User>? _usersRepository;
    private IRepository<Follow>? _followsRepository;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IRepository<User> Users => _usersRepository ??= new Repository<User>(_dbContext);
    
    public IRepository<Follow> Follows => _followsRepository ??= new Repository<Follow>(_dbContext);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
