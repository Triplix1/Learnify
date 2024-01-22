using IdentityService.Data;
using IdentityService.Models;
using IdentityService.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Repository;

public class ConfirmationRepository : IConfirmationRepository
{
    private readonly ApplicationDbContext _context;

    public ConfirmationRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        var confirmation = await _context.Confirmations.FirstOrDefaultAsync(c => c.Id == id);
        
        if (confirmation is null)
            return false;

        _context.Remove(confirmation);
        
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Confirmation> CreateAsync(Confirmation entity)
    {
        _context.Confirmations.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Confirmation?> GetByEmailAsync(string email)
    {
        return await _context.Confirmations.FirstOrDefaultAsync(c => c.Email == email);
    }
}