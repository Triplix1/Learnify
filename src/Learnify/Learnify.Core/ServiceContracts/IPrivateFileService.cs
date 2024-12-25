namespace Learnify.Core.ManagerContracts;

public interface IPrivateFileService
{
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}