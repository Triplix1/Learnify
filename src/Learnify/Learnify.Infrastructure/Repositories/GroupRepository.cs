using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly ApplicationDbContext _context;

    public GroupRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Group> GetByNameAsync(string name, IEnumerable<string> includes = null,
        CancellationToken cancellationToken = default)
    {
        if (includes == null || !includes.Any())
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _context.Groups.FindAsync(name);
        }


        var query = _context.Groups.AsQueryable();

        if (includes.Any())
            query = includes.Aggregate(query, (q, i) => q.Include(i));

        return await query.FirstOrDefaultAsync(g => g.Name == name, cancellationToken: cancellationToken);
    }

    public async Task<Group> CreateAsync(Group group, CancellationToken cancellationToken = default)
    {
        await _context.Groups.AddAsync(group, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return group;
    }

    public async Task<Group> UpdateAsync(Group group, CancellationToken cancellationToken = default)
    {
        _context.Groups.Update(group);

        await _context.SaveChangesAsync(cancellationToken);

        return group;
    }

    public async Task<Group> GetByConnectionIdAsync(string connectionId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups.FirstOrDefaultAsync(g => g.Connections.Any(c => c.ConnectionId == connectionId),
            cancellationToken: cancellationToken);
    }

    public async Task<Group> GetByMessageIdAsync(int messageId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups.SingleOrDefaultAsync(g => g.Messages.Any(m => m.Id == messageId),
            cancellationToken: cancellationToken);
    }
}