using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

public class MessageRepository: IMessageRepository
{
    private readonly ApplicationDbContext _context;

    public MessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Message> CreateAsync(Message message)
    {
        await _context.Messages.AddAsync(message);

        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<IEnumerable<Message>> GetMessagesForGroupAsync(string groupName, string[] stringsToInclude)
    {
        var messages = _context.Messages.Where(m => m.Group.Name == groupName);

        stringsToInclude.Aggregate(messages, (c, include) => c.Include(include));

        return messages;
    }
}