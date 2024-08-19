using Learnify.Core.Dto;

namespace Learnify.Core.ServiceContracts;

public interface IFileService
{
    Task<ApiResponse<Stream>> GetFileStreamById(int id, int userId);
}