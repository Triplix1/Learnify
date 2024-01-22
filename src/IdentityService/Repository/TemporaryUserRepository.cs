using IdentityService.Data;
using IdentityService.Models;
using IdentityService.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Repository;

public class TemporaryUserRepository : ITemporaryUserRepository
{
    private readonly ApplicationDbContext _context;

    public TemporaryUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TemporaryUser> GetByIdAsync(Guid id)
    {
        return await _context.TemporaryUsers.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TemporaryUser> CreateAsync(TemporaryUser entity)
    {
        _context.TemporaryUsers.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var temporaryUser = await _context.TemporaryUsers.FirstOrDefaultAsync(c => c.Id == id);
        
        if (temporaryUser is null)
            return false;

        _context.Remove(temporaryUser);
        
        return await _context.SaveChangesAsync() > 0;
    }
}