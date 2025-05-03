using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.Blob;
using Learnify.Core.Dto.File;
using Learnify.Core.Dto.Params;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Transactions;

namespace Learnify.Core.Services;

public class FileService : IFileService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IBlobStorage _blobStorage;
    private readonly IUserBoughtValidatorManager _userBoughtValidatorManager;
    private readonly IMapper _mapper;

    public FileService(IPsqUnitOfWork psqUnitOfWork,
        IBlobStorage blobStorage,
        IMongoUnitOfWork mongoUnitOfWork,
        IMapper mapper,
        IUserBoughtValidatorManager userBoughtValidatorManager)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _blobStorage = blobStorage;
        _mongoUnitOfWork = mongoUnitOfWork;
        _mapper = mapper;
        _userBoughtValidatorManager = userBoughtValidatorManager;
    }

    public async Task<FileStreamResponse> GetFileStreamById(int id, int? userId,
        CancellationToken cancellationToken = default)
    {
        var file = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(id, cancellationToken);

        if (file is null)
            throw new KeyNotFoundException("Cannot find file with such id");

        var protectedFile = file.CourseId.HasValue;

        if (protectedFile)
        {
            if (!userId.HasValue)
            {
                throw new UnauthorizedAccessException("This user doesn't have access to this file");
            }

            await _userBoughtValidatorManager.ValidateUserAccessToTheCourseAsync(userId.Value, file.CourseId.Value,
                cancellationToken: cancellationToken);
        }

        var blobParams = new GetBlobParams()
        {
            ContainerName = file.ContainerName,
            BlobName = file.BlobName,
            ContentType = MapContentType(file.ContentType),
        };
        
        var blobStreamResponse = await _blobStorage.GetBlobStreamAsync(blobParams, cancellationToken);

         var fileStreamResponse = _mapper.Map<FileStreamResponse>(blobStreamResponse);
         
         fileStreamResponse.Protected = protectedFile;
         
         return fileStreamResponse;
    }

    private string MapContentType(string contentType)
    {
        if (contentType.StartsWith("image/") || contentType.StartsWith("video/"))
        {
            return "application/octet-stream";
        }
        if (contentType.EndsWith(".m3u8", StringComparison.OrdinalIgnoreCase))
            return "application/vnd.apple.mpegurl";
        if (contentType.EndsWith(".ts", StringComparison.OrdinalIgnoreCase))
            return "video/MP2T";

        return contentType;
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
                throw new KeyNotFoundException("cannot find course with such id");

            if (courseAuthorId != userId)
                throw new UnauthorizedAccessException("You have not permissions to edit this course");
        }

        var privateFile = new PrivateFileCreateRequest()
        {
            CourseId = privateFileBlobCreateRequest.CourseId,
            ContentType = privateFileBlobCreateRequest.ContentType
        };

        if (isPublic)
        {
            privateFile.CourseId = null;
        }

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

        PrivateFileData fileResponse;
        using (var ts = TransactionScopeBuilder.CreateReadCommittedAsync())
        {
            fileResponse = await _psqUnitOfWork.PrivateFileRepository.CreateFileAsync(privateFile, cancellationToken);
            
            await _blobStorage.UploadAsync(blobDto, cancellationToken: cancellationToken);

            ts.Complete();
        }
        
        var response = _mapper.Map<PrivateFileDataResponse>(fileResponse);

        return response;
    }
}