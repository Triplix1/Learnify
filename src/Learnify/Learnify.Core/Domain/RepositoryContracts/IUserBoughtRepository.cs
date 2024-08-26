using Learnify.Core.Domain.Entities.Sql;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IUserBoughtRepository
{
    Task<bool> UserBoughtExists(int userId, int courseId);
    Task<UserBought> CreateAsync(UserBought userBoughtCreateRequest);
    Task<bool> DeleteAsync(int userId, int courseId);
}