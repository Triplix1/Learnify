using Learnify.Core.Dto.File;

namespace Learnify.Core.ManagerContracts;

public interface IPrivateFileManager
{
    Task DeleteAsync(int id);
    Task DeleteRangeAsync(IEnumerable<int> ids);
}