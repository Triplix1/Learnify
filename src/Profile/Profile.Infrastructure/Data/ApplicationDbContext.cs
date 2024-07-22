using Microsoft.EntityFrameworkCore;
using Profile.Core.Domain.Entities;

namespace Profile.Infrastructure.Data;

/// <inheritdoc />
public class ApplicationDbContext : DbContext
{
    /// <inheritdoc />
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    /// <summary>
    /// Users set
    /// </summary>
    public DbSet<User> Users { get; set; }
}