using AutoMapper;
using Learnify.Contracts;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.Subtitles;
using Learnify.Core.Enums;
using Learnify.Core.ManagerContracts;
using Learnify.Core.Transactions;
using MassTransit;

namespace Learnify.Core.Managers;

public class SubtitlesManager : ISubtitlesManager
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IBlobStorage _blobStorage;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public SubtitlesManager(IPsqUnitOfWork psqUnitOfWork, IBlobStorage blobStorage, IMapper mapper,
        IPublishEndpoint publishEndpoint)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _blobStorage = blobStorage;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<SubtitlesResponse> GetSubtitleByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var subtitle = await _psqUnitOfWork.SubtitlesRepository.GetByIdAsync(id, [], cancellationToken);

        var response = _mapper.Map<SubtitlesResponse>(subtitle);

        return response;
    }

    public async Task<SubtitlesResponse> CreateAsync(SubtitlesCreateRequest subtitlesCreateRequest,
        CancellationToken cancellationToken = default)
    {
        var subtitle = _mapper.Map<Subtitle>(subtitlesCreateRequest);

        subtitle = await _psqUnitOfWork.SubtitlesRepository.CreateAsync(subtitle, cancellationToken);

        var response = _mapper.Map<SubtitlesResponse>(subtitle);

        return response;
    }

    public async Task<IEnumerable<SubtitleReference>> CreateAsync(string fileBlobName, string containerName, IEnumerable<Language> subtitlesLanguages, Language primaryLanguage, int? courseId,
        CancellationToken cancellationToken = default)
    {
        var subtitlesCreateRequest = subtitlesLanguages.Select(s => new Subtitle
        {
            Language = s,
            TranscriptionFile = new PrivateFileData
            {
                CourseId = courseId
            },
            SubtitleFile = new PrivateFileData
            {
                CourseId = courseId
            }

        }).ToArray();

        var subtitles =
            await _psqUnitOfWork.SubtitlesRepository.CreateRangeAsync(subtitlesCreateRequest, cancellationToken);

        var subtitlesInfo = _mapper.Map<IEnumerable<SubtitleInfo>>(subtitles);

        var subtitlesGenerateRequest = new SubtitlesGenerateRequest
        {
            VideoBlobName = fileBlobName,
            VideoContainerName = containerName,
            SubtitleInfo = subtitlesInfo,
            PrimaryLanguage = primaryLanguage.ToString()
        };

        await _publishEndpoint.Publish(subtitlesGenerateRequest, cancellationToken);

        var result = _mapper.Map<IEnumerable<SubtitleReference>>(subtitles);

        return result;
    }


    public async Task<SubtitlesResponse> UpdateAsync(SubtitlesUpdateRequest subtitlesUpdateRequest,
        CancellationToken cancellationToken = default)
    {
        var subtitles =
            await _psqUnitOfWork.SubtitlesRepository.GetByIdAsync(subtitlesUpdateRequest.Id, [], cancellationToken);

        if (subtitles is null)
            throw new KeyNotFoundException("Cannot find subtitles with such id");

        var subtitle = _mapper.Map<Subtitle>(subtitlesUpdateRequest);

        if (subtitles.SubtitleFileId.HasValue && subtitles.SubtitleFileId != subtitlesUpdateRequest.FileId)
        {
            var oldFile =
                await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(subtitles.SubtitleFileId.Value, cancellationToken);

            using var ts = TransactionScopeBuilder.CreateRepeatableReadAsync();

            await _psqUnitOfWork.PrivateFileRepository.DeleteAsync(oldFile.Id, cancellationToken);

            subtitle = await _psqUnitOfWork.SubtitlesRepository.UpdateAsync(subtitle, cancellationToken);

            await _blobStorage.DeleteAsync(oldFile.ContainerName, oldFile.BlobName, cancellationToken);

            ts.Complete();
        }
        else
        {
            subtitle = await _psqUnitOfWork.SubtitlesRepository.UpdateAsync(subtitle, cancellationToken);
        }
        
        if (subtitles.SubtitleFileId.HasValue && subtitles.SubtitleFileId != subtitlesUpdateRequest.FileId)
        {
            var oldFile =
                await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(subtitles.SubtitleFileId.Value, cancellationToken);

            using var ts = TransactionScopeBuilder.CreateRepeatableReadAsync();

            await _psqUnitOfWork.PrivateFileRepository.DeleteAsync(oldFile.Id, cancellationToken);

            subtitle = await _psqUnitOfWork.SubtitlesRepository.UpdateAsync(subtitle, cancellationToken);

            await _blobStorage.DeleteAsync(oldFile.ContainerName, oldFile.BlobName, cancellationToken);

            ts.Complete();
        }
        else
        {
            subtitle = await _psqUnitOfWork.SubtitlesRepository.UpdateAsync(subtitle, cancellationToken);
        }


        var response = _mapper.Map<SubtitlesResponse>(subtitle);

        return response;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var subtitles = await _psqUnitOfWork.SubtitlesRepository.GetByIdAsync(id, [], cancellationToken);

        if (subtitles is null)
            return false;

        using var ts = TransactionScopeBuilder.CreateRepeatableReadAsync();

        if (subtitles.SubtitleFileId.HasValue)
        {
            var oldFile =
                await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(subtitles.SubtitleFileId.Value, cancellationToken);


            await _psqUnitOfWork.PrivateFileRepository.DeleteAsync(oldFile.Id, cancellationToken);

            var result = await _psqUnitOfWork.SubtitlesRepository.DeleteAsync(subtitles.Id, cancellationToken);

            await _blobStorage.DeleteAsync(oldFile.ContainerName, oldFile.BlobName, cancellationToken);

            return result;
        }

        var response = await _psqUnitOfWork.SubtitlesRepository.DeleteAsync(subtitles.Id, cancellationToken);

        ts.Complete();

        return response;
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        var subtitles = await _psqUnitOfWork.SubtitlesRepository.GetByIdsAsync(ids, [], cancellationToken: cancellationToken);

        if (subtitles is null)
            return false;

        var fileIds = subtitles.Where(s => s.SubtitleFileId.HasValue).Select(s => s.SubtitleFileId.Value);

        if (fileIds.Any())
        {
            var files = await _psqUnitOfWork.PrivateFileRepository.GetByIdsAsync(fileIds, cancellationToken);

            bool result;
            using (var ts = TransactionScopeBuilder.CreateReadCommittedAsync())
            {
                await _psqUnitOfWork.PrivateFileRepository.DeleteRangeAsync(fileIds, cancellationToken);

                result = await _psqUnitOfWork.SubtitlesRepository.DeleteRangeAsync(ids, cancellationToken);

                foreach (var file in files.Where(f => f.ContainerName != null && f.BlobName != null))
                    await _blobStorage.DeleteAsync(file.ContainerName, file.BlobName, cancellationToken);

                ts.Complete();
            }

            return result;
        }

        return await _psqUnitOfWork.SubtitlesRepository.DeleteRangeAsync(ids, cancellationToken);
    }
}