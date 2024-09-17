using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.Messages;
using Learnify.Core.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace Learnify.Core.Hubs;

public class MessageHub: Hub
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IGroupRepository _groupRepository;
    private readonly IConnectionRepository _connectionRepository;

    public MessageHub(IMessageRepository messageRepository, IUserRepository userRepository,
        IMapper mapper, IGroupRepository groupRepository, IConnectionRepository connectionRepository)
    {
        _mapper = mapper;
        _groupRepository = groupRepository;
        _connectionRepository = connectionRepository;
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var userId = Context.User.GetUserId();
        var groupName = httpContext.Request.Query["group"];
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        var group = await AddToGroup(groupName);

        await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

        var messages = await _messageRepository.GetMessagesForGroupAsync(groupName);

        await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
    }

    public override async Task OnDisconnectedAsync(Exception ex)
    {
        var group = await RemoveFromMessageGroup();
        await Clients.Group(group.Name).SendAsync("UpdatedGroup");
        await base.OnDisconnectedAsync(ex);
    }

    public async Task SendMessage(MessageCreateRequest messageCreateRequest)
    {
        var sender = await _userRepository.GetByIdAsync(messageCreateRequest.SenderId);
        
        if(sender is null)
            throw new HubException("Not found user");
        
        var group = await _groupRepository.GetByNameAsync(messageCreateRequest.GroupName);

        if (group is null)
            throw new HubException("cannot find group with specified name");
        
        var message = new Message
        {
            Sender = sender, 
            Content = messageCreateRequest.Content,
            GroupId = messageCreateRequest.GroupName
        };
        
        message = await _messageRepository.CreateAsync(message);

        if (message is null)
            throw new HubException("Failed while creating message");
        
        await Clients.Group(messageCreateRequest.GroupName).SendAsync("NewMessage", _mapper.Map<MessageResponse>(message));
    }
    
    private async Task<Group> AddToGroup(string groupName)
    {
        var group = await _groupRepository.GetByNameAsync(groupName);

        if (group == null)
        {
            group = new Group
            {
                Name = groupName
            };
            
            await _groupRepository.CreateAsync(group);
        }

        var connection = new Connection
        {
            UserId = Context.User.GetUserId()
        };
        
        group.Connections.Add(connection);

        group = await _groupRepository.UpdateAsync(group);

        if (group is null)
            throw new HubException("Cannot update specified group");
        
        return group;
    }

    private async Task<Group> RemoveFromMessageGroup()
    {
        var group = await _groupRepository.GetByConnectionIdAsync(Context.ConnectionId);

        if (group is null)
            throw new HubException("cannot find group with specified name");
        
        var removedSuccessfully = await _connectionRepository.RemoveAsync(Context.ConnectionId);
        
        if (!removedSuccessfully)
            throw new HubException("Cannot delete connection with such id");

        var userId = Context.User.GetUserId();

        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
            throw new HubException("Cannot find user with such id");

        var message = new Message()
        {
            Content = $"User {user.Username} leaved the room",
            Group = group
        };

        throw new HubException("Failed to remove from group");
    }
}