using Learnify.Core.Domain.Entities.Sql;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IUserBoughtRepository
{
    Task<bool> UserBoughtExistsAsync(int userId, int courseId, CancellationToken cancellationToken = default);
    Task<UserBought> CreateAsync(UserBought userBoughtCreateRequest, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int userId, int courseId, CancellationToken cancellationToken = default);
}