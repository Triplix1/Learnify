using AutoMapper;
using Learnify.Contracts;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.File;
using Learnify.Core.ManagerContracts;
using Learnify.Core.Transactions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Learnify.Core.Consumers;

public class SubtitlesGeneratedResponseConsumer : IConsumer<SubtitlesGeneratedResponse>
{
    private readonly ILogger<SubtitlesGeneratedResponseConsumer> _logger;
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IBlobStorage _blobStorage;
    private readonly IMapper _mapper;
    private readonly ISubtitlesManager _subtitlesManager;

    public SubtitlesGeneratedResponseConsumer(ILogger<SubtitlesGeneratedResponseConsumer> logger,
        IPsqUnitOfWork psqUnitOfWork,
        IMapper mapper,
        IBlobStorage blobStorage,
        ISubtitlesManager subtitlesManager,
        IMongoUnitOfWork mongoUnitOfWork)
    {
        _logger = logger;
        _psqUnitOfWork = psqUnitOfWork;
        _mapper = mapper;
        _blobStorage = blobStorage;
        _subtitlesManager = subtitlesManager;
        _mongoUnitOfWork = mongoUnitOfWork;
    }

    public async Task Consume(ConsumeContext<SubtitlesGeneratedResponse> context)
    {
        _logger.LogInformation("Received Subtitles Generated Response");
        var message = context.Message;

        var subtitle = await _psqUnitOfWork.SubtitlesRepository.GetByIdAsync(message.SubtitleId,
            [nameof(Subtitle.SubtitleFile)]);

        if (subtitle == null)
        {
            _logger.LogInformation("Seems like subtitle with specified id were deleted");
            await _blobStorage.DeleteAsync(message.SubtitleFileInfo.ContainerName, message.SubtitleFileInfo.BlobName);
            return;
        }

        using (var transaction = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            subtitle.SubtitleFile = await HandleUpdateOfFile(subtitle.SubtitleFile, message.SubtitleFileInfo);

            await _psqUnitOfWork.SubtitlesRepository.UpdateAsync(subtitle);

            transaction.Complete();
        }

        var subtitles = await _mongoUnitOfWork.Lessons.GetSubtitleReferencesForLessonAsync(message.LessonId);

        var subtitlesToTranslate = subtitles.Where(s => s.SubtitleId != subtitle.Id).Select(s => s.SubtitleId);

        await _subtitlesManager.RequestSubtitlesTranslationAsync(subtitle.Id, subtitlesToTranslate);
    }

    private async Task<PrivateFileData> HandleUpdateOfFile(PrivateFileData file,
        GeneratedResponseUpdateRequest generatedResponseUpdateRequest)
    {
        if (file is null)
        {
            var transcriptionFileCreateRequest = _mapper.Map<PrivateFileCreateRequest>(generatedResponseUpdateRequest);
            return await _psqUnitOfWork.PrivateFileRepository.CreateFileAsync(transcriptionFileCreateRequest);
        }

        _mapper.Map(generatedResponseUpdateRequest, file);
        return await _psqUnitOfWork.PrivateFileRepository.UpdateFileAsync(file);
    }
}