using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.Entities.Sql;
using Microsoft.EntityFrameworkCore;
using Course = Learnify.Core.Domain.Entities.Sql.Course;

namespace Learnify.Infrastructure.Data;

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
    
    /// <summary>
    /// CourseRatings Db Set
    /// </summary>
    public DbSet<CourseRating> CourseRatings { get; set; }
    
    /// <summary>
    /// Courses DbSet
    /// </summary>
    public DbSet<Course> Courses { get; set; }
    
    /// <summary>
    /// Courses DbSet
    /// </summary>
    public DbSet<Paragraph> Paragraphs { get; set; }
    
    /// <summary>
    /// Courses DbSet
    /// </summary>
    public DbSet<PrivateFileData> FileDatas { get; set; }
    
    /// <summary>
    /// Subtitles DbSet
    /// </summary>
    public DbSet<Subtitle> Subtitles { get; set; }
    
    /// <summary>
    /// UserBoughts DbSet
    /// </summary>
    public DbSet<UserBought> UserBoughts { get; set; }
    
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RefreshToken>().HasIndex(rt => rt.Jwt);
        builder.Entity<User>().HasIndex(u => u.Email);
        builder.Entity<CourseRating>().HasIndex(r => r.CourseId);
        builder.Entity<UserBought>().HasKey(ub => new { ub.UserId, ub.CourseId });
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