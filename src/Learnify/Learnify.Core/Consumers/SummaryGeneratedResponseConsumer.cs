using AutoMapper;
using Learnify.Contracts;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.ManagerContracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Learnify.Core.Consumers;

public class SummaryGeneratedResponseConsumer : IConsumer<SummaryGeneratedResponse>
{
    private readonly ILogger<SubtitlesGeneratedResponseConsumer> _logger;
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IBlobStorage _blobStorage;
    private readonly IMapper _mapper;

    public SummaryGeneratedResponseConsumer(ILogger<SubtitlesGeneratedResponseConsumer> logger,
        IPsqUnitOfWork psqUnitOfWork,
        IMongoUnitOfWork mongoUnitOfWork,
        IBlobStorage blobStorage,
        IMapper mapper)
    {
        _logger = logger;
        _psqUnitOfWork = psqUnitOfWork;
        _mongoUnitOfWork = mongoUnitOfWork;
        _blobStorage = blobStorage;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<SummaryGeneratedResponse> context)
    {
        _logger.LogInformation("Received Summary Generated Response");

        var message = context.Message;

        var file = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(message.FileId);

        if (file == null)
        {
            _logger.LogInformation("Summary for file with id: {FileId} not found", message.FileId);
            await _blobStorage.DeleteAsync(message.SummaryFileInfo.ContainerName, message.SummaryFileInfo.BlobName);
        }

        _mapper.Map(message.SummaryFileInfo, file);

        await _psqUnitOfWork.PrivateFileRepository.UpdateFileAsync(file);

        _logger.LogInformation("Proceesed Summary Generated Response");
    }
}