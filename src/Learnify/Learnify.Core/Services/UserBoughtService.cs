using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.UserBought;
using Learnify.Core.ServiceContracts;

namespace Learnify.Core.Services;

public class UserBoughtService: IUserBoughtService
{
    private readonly IUserBoughtRepository _userBoughtRepository;
    private readonly IMapper _mapper;

    public UserBoughtService(IUserBoughtRepository userBoughtRepository, IMapper mapper)
    {
        _userBoughtRepository = userBoughtRepository;
        _mapper = mapper;
    }

    public async Task<UserBoughtResponse> SaveSucceedCourseBoughtResultAsync(UserBoughtCreateRequest userBoughtCreateRequest,
        CancellationToken cancellationToken)
    {
        var userBought = new UserBought()
        {
            CourseId = userBoughtCreateRequest.CourseId,
            UserId = userBoughtCreateRequest.UserId,
        };

        userBought = await _userBoughtRepository.CreateAsync(userBought, cancellationToken: cancellationToken);
        
        return _mapper.Map<UserBoughtResponse>(userBought);
    }
}