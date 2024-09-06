using Learnify.Core.Dto;
using Learnify.Core.Dto.File;

namespace Learnify.Core.ServiceContracts;

public interface IFileService
{
    Task<FileStreamResponse> GetFileStreamById(int id, int userId);
    Task<ApiResponse<PrivateFileDataResponse>> CreateAsync(PrivateFileBlobCreateRequest privateFileBlobCreateRequest, int userId);
}