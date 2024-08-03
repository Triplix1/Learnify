using Learnify.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure;

/// <inheritdoc />
public class ApplicationDbContext: DbContext
{
    /// <inheritdoc />
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }
    
    /// <summary>
    /// RefreshTokens Db Set
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    /// <summary>
    /// Users Db Set
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RefreshToken>().HasIndex(rt => rt.Jwt);
        builder.Entity<User>().HasIndex(u => u.Email);
    }

    /// <inheritdoc />
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