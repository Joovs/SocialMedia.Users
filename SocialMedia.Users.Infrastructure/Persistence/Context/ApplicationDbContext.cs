using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Domain.Entities.UserEntity;

namespace SocialMedia.Users.Infrastructure.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets (referenciando a las tablas de la base de datos)
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Posts> Posts { get; set; }
}
