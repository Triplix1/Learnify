using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Blob;
using Learnify.Core.Dto.File;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Transactions;

namespace Learnify.Core.Services;

public class FileService : IFileService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IBlobStorage _blobStorage;
    private readonly IMapper _mapper;

    public FileService(IPsqUnitOfWork psqUnitOfWork, IBlobStorage blobStorage, IMongoUnitOfWork mongoUnitOfWork,
        IMapper mapper)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _blobStorage = blobStorage;
        _mongoUnitOfWork = mongoUnitOfWork;
        _mapper = mapper;
    }

    public async Task<FileStreamResponse> GetFileStreamById(int id, int userId,
        CancellationToken cancellationToken = default)
    {
        var file = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(id, cancellationToken);

        if (file is null)
            throw new KeyNotFoundException("Cannot find file with such id");

        if (file.CourseId.HasValue)
        {
            var courseAuthorId =
                await _psqUnitOfWork.CourseRepository.GetAuthorIdAsync(file.CourseId.Value, cancellationToken);

            if (courseAuthorId is null)
                ApiResponse.Failure(new KeyNotFoundException("cannot find course with such id"));

            //Author should have access without buying the course
            if (courseAuthorId != userId)
            {
                var userHasBoughtThisCourse =
                    await _psqUnitOfWork.UserBoughtRepository.UserBoughtExistsAsync(userId, file.CourseId.Value,
                        cancellationToken);

                if (!userHasBoughtThisCourse)
                    throw new Exception("User has not access for this file");
            }
        }

        var stream = await _blobStorage.GetBlobStreamAsync(file.ContainerName, file.BlobName, cancellationToken);

        return stream;
    }

    public async Task<PrivateFileDataResponse> CreateAsync(
        PrivateFileBlobCreateRequest privateFileBlobCreateRequest, bool isPublic, int userId,
        CancellationToken cancellationToken = default)
    {
        if (privateFileBlobCreateRequest.CourseId.HasValue && !isPublic)
        {
            var courseAuthorId =
                await _psqUnitOfWork.CourseRepository.GetAuthorIdAsync(privateFileBlobCreateRequest.CourseId.Value,
                    cancellationToken);

            if (courseAuthorId is null)
                ApiResponse.Failure(new KeyNotFoundException("cannot find course with such id"));

            if (courseAuthorId != userId)
                ApiResponse.Failure(new UnauthorizedAccessException("You have not permissions to edit this course"));
        }

        var privateFile = new PrivateFileData()
        {
            CourseId = privateFileBlobCreateRequest.CourseId,
            ContentType = privateFileBlobCreateRequest.ContentType
        };

        var container = "learnify";
        var blobName = new string(Guid.NewGuid().ToString().Except("-").ToArray());

        privateFile.ContainerName = container;
        privateFile.BlobName = blobName;

        await using var stream = privateFileBlobCreateRequest.Content.OpenReadStream();

        var blobDto = new BlobDto()
        {
            ContainerName = container,
            Name = blobName,
            Content = stream,
            ContentType = privateFileBlobCreateRequest.ContentType
        };

        using var ts = TransactionScopeBuilder.CreateReadCommittedAsync();

        var fileResponse = await _psqUnitOfWork.PrivateFileRepository.CreateFileAsync(privateFile, cancellationToken);

        await _blobStorage.UploadAsync(blobDto, cancellationToken: cancellationToken);

        ts.Complete();

        var response = _mapper.Map<PrivateFileDataResponse>(fileResponse);

        return response;
    }
    
    public async Task<UrlResponse> GetHlsManifestUrl(int id, int userId,
        CancellationToken cancellationToken = default)
    {
        var file = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(id, cancellationToken);

        if (file is null)
            throw new KeyNotFoundException("Cannot find file with such id");

        if (file.CourseId.HasValue)
        {
            var courseAuthorId =
                await _psqUnitOfWork.CourseRepository.GetAuthorIdAsync(file.CourseId.Value, cancellationToken);

            if (courseAuthorId is null)
                ApiResponse.Failure(new KeyNotFoundException("cannot find course with such id"));

            //Author should have access without buying the course
            if (courseAuthorId != userId)
            {
                var userHasBoughtThisCourse =
                    await _psqUnitOfWork.UserBoughtRepository.UserBoughtExistsAsync(userId, file.CourseId.Value,
                        cancellationToken);

                if (!userHasBoughtThisCourse)
                    throw new Exception("User has not access for this file");
            }
        }

        var url = await _blobStorage.GetHlsManifestUrl(file.ContainerName, file.BlobName, cancellationToken);

        return new UrlResponse() { Url = url };
    }
}