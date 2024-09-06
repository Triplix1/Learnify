using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Blob;
using Learnify.Core.Dto.File;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Transactions;

namespace Learnify.Core.Services;

public class FileService: IFileService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IMongoUnitOfWork _mongoUnitOfWork;
    private readonly IBlobStorage _blobStorage;
    private readonly IMapper _mapper;

    public FileService(IPsqUnitOfWork psqUnitOfWork, IBlobStorage blobStorage, IMongoUnitOfWork mongoUnitOfWork, IMapper mapper)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _blobStorage = blobStorage;
        _mongoUnitOfWork = mongoUnitOfWork;
        _mapper = mapper;
    }

    public async Task<FileStreamResponse> GetFileStreamById(int id, int userId)
    {
        var file = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(id);

        if (file is null)
            throw new KeyNotFoundException("Cannot find file with such id");

        if (file.CourseId.HasValue)
        {
            var courseAuthorId = await _psqUnitOfWork.CourseRepository.GetAuthorId(file.CourseId.Value);

            if (courseAuthorId is null)
                ApiResponse.Failure(new KeyNotFoundException("cannot find course with such id"));

            //Author should have access without buying the course
            if (courseAuthorId != userId)
            {
                var userHasBoughtThisCourse = await _psqUnitOfWork.UserBoughtRepository.UserBoughtExists(userId, file.CourseId.Value);

                if (!userHasBoughtThisCourse)
                    throw new Exception("User has not access for this file");
            }
        }

        var stream = await _blobStorage.GetBlobStreamAsync(file.ContainerName, file.BlobName);

        return stream;
    }
    
    public async Task<ApiResponse<PrivateFileDataResponse>> CreateAsync(PrivateFileBlobCreateRequest privateFileBlobCreateRequest, int userId)
    {
        if (privateFileBlobCreateRequest.CourseId.HasValue)
        {
            var courseAuthorId = await _psqUnitOfWork.CourseRepository.GetAuthorId(privateFileBlobCreateRequest.CourseId.Value);

            if (courseAuthorId is null)
                ApiResponse.Failure(new KeyNotFoundException("cannot find course with such id"));
            
            if (courseAuthorId != userId)
                ApiResponse.Failure(new UnauthorizedAccessException("You have not permissions to edit this course"));
        }
        
        var privateFile = _mapper.Map<PrivateFileData>(privateFileBlobCreateRequest);

        var container = "Learnify";
        var blobName = Guid.NewGuid().ToString();

        privateFile.ContainerName = container;
        privateFile.BlobName = blobName;
        
        await using var stream = privateFileBlobCreateRequest.Content.OpenReadStream();

        byte[] b;

        using (BinaryReader br = new BinaryReader(stream))
        {
            b = br.ReadBytes((int)stream.Length);
        }
        
        var blobDto = new BlobDto()
        {
            ContainerName = container,
            Name = blobName,
            Content = b,
            ContentType = privateFileBlobCreateRequest.ContentType
        };
        
        using var ts = TransactionScopeBuilder.CreateReadCommittedAsync();

        var fileResponse = await _psqUnitOfWork.PrivateFileRepository.CreateFileAsync(privateFile);
        
        await _blobStorage.UploadAsync(blobDto);
        
        ts.Complete();

        var response = _mapper.Map<PrivateFileDataResponse>(fileResponse);

        return ApiResponse<PrivateFileDataResponse>.Success(response);
    }
}