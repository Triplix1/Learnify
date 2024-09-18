using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.Messages;
using Learnify.Core.ServiceContracts;

namespace Learnify.Core.Services;

public class MessagesService: IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;

    public MessagesService(IMessageRepository messageRepository, IMapper mapper)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MessageResponse>> GetMessagesForGroupAsync(string groupName)
    {
        var messages = await _messageRepository.GetMessagesForGroupAsync(groupName, new []{nameof(Message.Group), nameof(Message.Sender)});

        return _mapper.Map<IEnumerable<MessageResponse>>(messages);
    }
}