using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Domain.Entities.FollowEntity;
using SocialMedia.Users.Domain.Entities.UserEntity;

namespace SocialMedia.Users.Infrastructure.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Follow>(entity =>
        {
            entity.ToTable("Follows"); 

            entity.HasKey(x => new { x.FollowerId, x.FollowingId });

            entity.Property(x => x.CreatedAt)
                  .IsRequired();
        });
    }


    // DbSets (referenciando a las tablas de la base de datos)
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Follow> Follows { get; set; }


}
