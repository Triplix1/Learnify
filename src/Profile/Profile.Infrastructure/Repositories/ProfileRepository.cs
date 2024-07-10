using Microsoft.EntityFrameworkCore;
using Profile.Core.Domain.Entities;
using Profile.Core.Domain.RepositoryContracts;
using Profile.Infrastructure.Data;

namespace Profile.Infrastructure.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly ApplicationDbContext _context;

    public ProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task CreateAsync(User entity)
    {
        await _context.Users.AddAsync(entity);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(User entity)
    {
        if (entity is null)
            return false;

        _context.Users.Remove(entity);

        return (await _context.SaveChangesAsync()) > 0;
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> UpdateAsync(User entity)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == entity.Id);

        if (user is null)
            return null;

        user.Name = entity.Name;
        user.Company = entity.Company;
        user.Surname = entity.Surname;
        user.CardNumber = entity.CardNumber;
        user.PhotoUrl = entity.PhotoUrl;
        user.PhotoPublicId = entity.PhotoPublicId;

        await _context.SaveChangesAsync();

        return user;
    }
}