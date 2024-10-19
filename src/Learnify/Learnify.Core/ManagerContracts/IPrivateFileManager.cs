namespace Learnify.Core.ManagerContracts;

public interface IPrivateFileManager
{
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}