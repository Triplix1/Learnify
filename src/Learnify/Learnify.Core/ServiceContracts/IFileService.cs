using Learnify.Core.Dto;

namespace Learnify.Core.ServiceContracts;

public interface IFileService
{
    Task<Stream> GetFileStreamById(int id, int userId);
}