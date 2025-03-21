﻿using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.Entities.Sql;
using Microsoft.EntityFrameworkCore;
using Course = Learnify.Core.Domain.Entities.Sql.Course;

namespace Learnify.Infrastructure.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<CourseRating> CourseRatings { get; set; }
    
    public DbSet<Course> Courses { get; set; }
    
    public DbSet<Paragraph> Paragraphs { get; set; }
    
    public DbSet<PrivateFileData> FileDatas { get; set; }
    
    public DbSet<Subtitle> Subtitles { get; set; }
    
    public DbSet<UserBought> UserBoughts { get; set; }
    
    public DbSet<Group> Groups { get; set; }
    
    public DbSet<Message> Messages { get; set; }
    
    public DbSet<Connection> Connections { get; set; }
    
    public DbSet<UserQuizAnswer> UserQuizAnswers { get; set; }

    public DbSet<MeetingSession> MeetingSessions { get; set; }
    public DbSet<MeetingConnection> MeetingConnections { get; set; }
    public DbSet<MeetingStream> MeetingStreams { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RefreshToken>().HasIndex(rt => rt.Jwt);
        builder.Entity<User>().HasIndex(u => u.Email);
        builder.Entity<CourseRating>().HasIndex(r => r.CourseId);
        builder.Entity<UserBought>().HasKey(ub => new { ub.UserId, ub.CourseId });
        builder.Entity<UserQuizAnswer>().HasKey(ub => new { ub.UserId, ub.LessonId, ub.QuizId });
        builder.Entity<UserQuizAnswer>().HasIndex(ub => new { ub.UserId, ub.LessonId });
        builder.Entity<MeetingSession>().HasIndex(s => s.CourseId);
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