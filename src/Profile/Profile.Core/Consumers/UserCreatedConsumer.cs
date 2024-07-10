using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Profile.Core.Domain.Entities;
using Profile.Core.Domain.RepositoryContracts;

namespace Profile.Core.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreated>
{
    private readonly IMapper _mapper;
    private readonly IProfileRepository _profileRepository;
    private readonly ILogger<UserCreatedConsumer> _logger;

    public UserCreatedConsumer(IMapper mapper, IProfileRepository profileRepository, ILogger<UserCreatedConsumer> logger)
    {
        _mapper = mapper;
        _profileRepository = profileRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        _logger.LogInformation($"Trying to create user with email: {context.Message.Email}");
        
        var user = _mapper.Map<User>(context.Message);
        
        await _profileRepository.CreateAsync(user);
    }
}