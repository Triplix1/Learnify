using AutoMapper;
using BlobStorage.Core.Models;
using BlobStorage.Core.Services.Contracts;
using ClassLibrary1.Blob;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BlobStorage.Core.Consumers;

public class AddBlobConsumer : IConsumer<BlobAddRequest>
{
    private readonly IBlobStorage _blobStorage;
    private readonly IMapper _mapper;
    private readonly ILogger<AddBlobConsumer> _logger;

    public AddBlobConsumer(IBlobStorage blobStorage, IMapper mapper, ILogger<AddBlobConsumer> logger)
    {
        _blobStorage = blobStorage;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BlobAddRequest> context)
    {
        _logger.LogInformation($"Trying to add blob: {context.Message.Name} to container: {context.Message.ContainerName}");
        
        var blobDto = _mapper.Map<BlobDto>(context.Message);
        var blob = await _blobStorage.UploadAsync(blobDto);

        var response = _mapper.Map<BlobCreatedResponse>(blob);

        await context.RespondAsync(response);
    }
}