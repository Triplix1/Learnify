using Learnify.Core.Dto.UserBought;

namespace Learnify.Core.ServiceContracts;

public interface IUserBoughtService
{
    Task<UserBoughtResponse> SaveSucceedCourseBoughtResultAsync(UserBoughtCreateRequest userBoughtCreateRequest, CancellationToken cancellationToken);
}