using Learnify.Core.Domain.Entities.Sql;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IMessageRepository
{
    Task<Message> GetByIdAsync(int messageId, IEnumerable<string> includes = null, CancellationToken cancellationToken = default);
    Task<Message> CreateAsync(Message message, CancellationToken cancellationToken = default);
    Task<IEnumerable<Message>> GetMessagesForGroupAsync(string groupName, string[] stringsToInclude = null, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int messageId, CancellationToken cancellationToken = default);
}