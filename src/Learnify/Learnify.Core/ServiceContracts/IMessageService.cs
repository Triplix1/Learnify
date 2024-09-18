using Learnify.Core.Dto;
using Learnify.Core.Dto.Messages;

namespace Learnify.Core.ServiceContracts;

public interface IMessageService
{
    Task<IEnumerable<MessageResponse>> GetMessagesForGroupAsync(string groupName);
}