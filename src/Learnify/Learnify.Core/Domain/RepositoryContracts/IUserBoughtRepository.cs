using Learnify.Core.Dto.UserBought;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IUserBoughtRepository
{
    Task<bool> UserBoughtExists(int userId, int courseId);
    Task<UserBoughtResponse> CreateAsync(UserBoughtCreateRequest userBoughtCreateRequest);
    Task<bool> DeleteAsync(int userId, int courseId);
}