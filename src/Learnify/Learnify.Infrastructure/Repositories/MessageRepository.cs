using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationDbContext _context;

    public MessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Message> GetByIdAsync(int messageId, IEnumerable<string> includes = null,
        CancellationToken cancellationToken = default)
    {
        if (includes is null || !includes.Any())
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _context.Messages.FindAsync(messageId);
        }

        var messages = _context.Messages.AsQueryable();

        includes.Aggregate(messages, (m, prop) => m.Include(prop));

        return await messages.FirstOrDefaultAsync(m => m.Id == messageId, cancellationToken: cancellationToken);
    }

    public async Task<Message> CreateAsync(Message message, CancellationToken cancellationToken = default)
    {
        await _context.Messages.AddAsync(message, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return message;
    }

    public async Task<IEnumerable<Message>> GetMessagesForGroupAsync(string groupName, string[] stringsToInclude = null,
        CancellationToken cancellationToken = default)
    {
        var messages = _context.Messages.Where(m => m.Group.Name == groupName);

        if (stringsToInclude is not null)
            messages = stringsToInclude.Aggregate(messages, (c, include) => c.Include(include));

        return await messages.ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> DeleteAsync(int messageId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var message = await _context.Messages.FindAsync(messageId);

        if (message is null)
            return false;

        _context.Remove(message);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}