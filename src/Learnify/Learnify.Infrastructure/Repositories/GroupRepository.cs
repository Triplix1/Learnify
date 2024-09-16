using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

public class GroupRepository: IGroupRepository
{
    private readonly ApplicationDbContext _context;

    public GroupRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Group> GetByNameAsync(string name)
    {
        return await _context.Groups.FindAsync(name);
    }

    public async Task<Group> CreateAsync(Group group)
    {
        await _context.Groups.AddAsync(group);

        await _context.SaveChangesAsync();

        return group;
    }

    public async Task<Group> UpdateAsync(Group group)
    {
        _context.Groups.Update(group);

        await _context.SaveChangesAsync();

        return group;
    }

    public async Task<Group> GetByConnectionIdAsync(string connectionId)
    {
        return await _context.Groups.FirstOrDefaultAsync(g => g.Connections.Any(c => c.ConnectionId == connectionId));
    }
}