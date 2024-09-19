using Learnify.Core.Domain.Entities.Sql;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IMessageRepository
{
    Task<Message> GetByIdAsync(int messageId, IEnumerable<string> includes = null);
    Task<Message> CreateAsync(Message message);
    Task<IEnumerable<Message>> GetMessagesForGroupAsync(string groupName, string[] stringsToInclude = null);
    Task<bool> DeleteAsync(int messageId);
}