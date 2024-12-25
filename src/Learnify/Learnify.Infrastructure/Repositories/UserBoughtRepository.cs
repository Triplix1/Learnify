using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data;

namespace Learnify.Infrastructure.Repositories;

public class UserBoughtRepository : IUserBoughtRepository
{
    private readonly ApplicationDbContext _context;

    public UserBoughtRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UserBoughtExistsAsync(int userId, int courseId, CancellationToken cancellationToken = default)
    {
        var userBought = await _context.UserBoughts.FindAsync([userId, courseId], cancellationToken);

        return userBought is not null;
    }

    public async Task<UserBought> CreateAsync(UserBought userBoughtCreateRequest,
        CancellationToken cancellationToken = default)
    {
        await _context.UserBoughts.AddAsync(userBoughtCreateRequest, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return userBoughtCreateRequest;
    }

    public async Task<bool> DeleteAsync(int userId, int courseId, CancellationToken cancellationToken = default)
    {
        var userBought = await _context.UserBoughts.FindAsync([userId, courseId], cancellationToken);

        if (userBought is null)
            return false;

        _context.UserBoughts.Remove(userBought);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}