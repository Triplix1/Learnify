using Learnify.Core.Domain.Entities.Sql;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IMessageRepository
{
    Task<Message> CreateAsync(Message message);
    Task<IEnumerable<Message>> GetMessagesForGroupAsync(string groupName);
}