using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data;

namespace Learnify.Infrastructure.Repositories;

public class ConnectionRepository : IConnectionRepository
{
    private readonly ApplicationDbContext _context;

    public ConnectionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RemoveAsync(string connectionId, CancellationToken cancellationToken = default)
    {
        var connection = await _context.Connections.FindAsync([connectionId], cancellationToken);

        if (connection is null)
            return false;

        _context.Connections.Remove(connection);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}