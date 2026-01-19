using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Domain.Entities.FollowsEntity;
using SocialMedia.Users.Domain.Entities.UserEntity;

namespace SocialMedia.Users.Infrastructure.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Follows>()
            .HasKey(f => new { f.FollowerId, f.FollowingId });
    }

    // DbSets (referenciando a las tablas de la base de datos)
    public required virtual DbSet<User> Users { get; set; }
    public required virtual DbSet<Follows> Follows { get; set; }


}
