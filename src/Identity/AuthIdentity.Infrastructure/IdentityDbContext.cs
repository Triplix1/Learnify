using AuthIdentity.Core.Domain.Entities;
using General.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuthIdentity.Infrastructure;

public class IdentityDbContext: DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options): base(options)
    {
    }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RefreshToken>().HasIndex(rt => rt.Jwt);
        builder.Entity<User>().HasIndex(u => u.Email);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
                ((BaseEntity)entity.Entity).CreatedAt = DateTime.UtcNow;
            }

            ((BaseEntity)entity.Entity).UpdatedAt = DateTime.UtcNow;
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}