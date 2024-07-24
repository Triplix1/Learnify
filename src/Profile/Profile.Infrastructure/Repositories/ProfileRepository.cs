using Microsoft.EntityFrameworkCore;
using Profile.Core.Domain.Entities;
using Profile.Core.Domain.RepositoryContracts;
using Profile.Infrastructure.Data;

namespace Profile.Infrastructure.Repositories;

/// <inheritdoc />
public class ProfileRepository : IProfileRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of ProfileRepository
    /// </summary>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    public ProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<User> CreateAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
        return entity;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(User entity)
    {
        if (entity is null)
            return false;

        _context.Users.Remove(entity);
        return true;
    }

    /// <inheritdoc />
    public async Task<User?> GetByIdAsync(string id)
    {
        return await _context.Users.FindAsync(id);
    }

    /// <inheritdoc />
    public async Task<User?> UpdateAsync(User entity)
    {
        var user = await _context.Users.FindAsync(entity.Id);

        if (user is null)
            return null;

        user.Name = entity.Name;
        user.Company = entity.Company;
        user.Surname = entity.Surname;
        user.CardNumber = entity.CardNumber;
        user.ImageContainerName = entity.ImageContainerName;
        user.ImageUrl = entity.ImageUrl;
        user.ImageBlobName = entity.ImageBlobName;
        
        return user;
    }
}