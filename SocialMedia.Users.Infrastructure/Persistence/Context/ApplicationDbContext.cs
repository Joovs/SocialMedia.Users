using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities;
using SocialMedia.Users.Application.Abstractions;

namespace SocialMedia.Users.Infrastructure.Persistence.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets (referenciando a las tablas de la base de datos)
    public virtual DbSet<User> Users { get; set; }

    // Tabla para seguir usuarios
    public virtual DbSet<Follow> Follows { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
