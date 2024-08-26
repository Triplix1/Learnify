using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data;

namespace Learnify.Infrastructure.Repositories;

public class UserBoughtRepository: IUserBoughtRepository
{
    private readonly ApplicationDbContext _context;

    public UserBoughtRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UserBoughtExists(int userId, int courseId)
    {
        var userBought = await _context.UserBoughts.FindAsync(userId, courseId);
        
        return userBought is not null;
    }

    public async Task<UserBought> CreateAsync(UserBought userBoughtCreateRequest)
    {
        await _context.UserBoughts.AddAsync(userBoughtCreateRequest);
        await _context.SaveChangesAsync();

        return userBoughtCreateRequest;
    }

    public async Task<bool> DeleteAsync(int userId, int courseId)
    {
        var userBought = await _context.UserBoughts.FindAsync(userId, courseId);

        if (userBought is null)
            return false;

        _context.UserBoughts.Remove(userBought);
        await _context.SaveChangesAsync();

        return true;
    }
}