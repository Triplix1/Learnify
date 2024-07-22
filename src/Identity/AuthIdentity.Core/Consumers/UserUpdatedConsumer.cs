using AuthIdentity.Core.Domain.Entities;
using AuthIdentity.Core.Domain.RepositoryContracts;
using AutoMapper;
using Contracts;
using Contracts.User;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuthIdentity.Core.Consumers;

public class UserUpdatedConsumer: IConsumer<UserUpdated>
{
    private readonly ILogger<UserUpdatedConsumer> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserUpdatedConsumer(ILogger<UserUpdatedConsumer> logger, IUserRepository userRepository, IMapper mapper)
    {
        _logger = logger;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<UserUpdated> context)
    {
        _logger.LogError($"Updating user with id: {context.Message.Id}");

        var user = _mapper.Map<User>(context.Message);

        var userInDb = await _userRepository.GetByIdAsync(user.Id);

        if (userInDb is null)
            throw new KeyNotFoundException("Cannot find user with such id");

        await _userRepository.UpdateAsync(user);
    }
}