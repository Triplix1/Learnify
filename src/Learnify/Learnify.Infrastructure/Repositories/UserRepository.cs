using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto;
using Learnify.Core.Enums;
using Learnify.Core.Extensions;
using Learnify.Core.Specification.Filters;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

/// <summary>
/// UserRepository
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<User>> GetPagedAsync(EfFilter<User> filter,
        CancellationToken cancellationToken = default)
    {
        var usersQuery = _context.Users.AsQueryable();

        usersQuery = usersQuery.ApplyFilters(filter);

        return await PagedList<User>.CreateAsync(usersQuery, filter.PagedListParams.PageNumber,
            filter.PagedListParams.PageSize, cancellationToken);
    }

    public async Task<Role> GetUserRoleByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users.Where(u => u.Id == userId).Select(s => s.Role).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username,
            cancellationToken: cancellationToken);
    }

    public async Task<User> GetByIdAsync(int id, IEnumerable<string> stringsToInclude, CancellationToken cancellationToken = default)
    {
        return await _context.Users.IncludeFields(stringsToInclude)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(user, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        
        return user;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var origin = await _context.Users.FindAsync([user.Id], cancellationToken);
        
        _context.Entry(origin).CurrentValues.SetValues(user);
        await _context.SaveChangesAsync(cancellationToken);

        return origin;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FindAsync([id], cancellationToken);

        if (user == null)
            return false;
        
        _context.Users.Remove(user);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}