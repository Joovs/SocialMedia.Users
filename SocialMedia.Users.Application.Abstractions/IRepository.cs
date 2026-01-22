using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Users.Application.Abstractions;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> FindAsync(params object?[]? keyValues);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    IQueryable<TEntity> AsQueryable();
}
