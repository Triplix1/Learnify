using AutoMapper;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
using Learnify.Core.Dto.File;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;

namespace Learnify.Core.Services;

public class FileService: IFileService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IBlobStorage _blobStorage;
    private readonly IMapper _mapper;

    public FileService(IPsqUnitOfWork psqUnitOfWork, IBlobStorage blobStorage, IMapper mapper)
    {
        _psqUnitOfWork = psqUnitOfWork;
        _blobStorage = blobStorage;
        _mapper = mapper;
    }

    public async Task<ApiResponse<Stream>> GetFileStreamById(int id, int courseId)
    {
        var file = await _psqUnitOfWork.PrivateFileRepository.GetByIdAsync(id);
        
        if()
    }
}