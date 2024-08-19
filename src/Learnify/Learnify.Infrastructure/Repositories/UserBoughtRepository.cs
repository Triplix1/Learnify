using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.UserBought;
using Learnify.Infrastructure.Data;

namespace Learnify.Infrastructure.Repositories;

public class UserBoughtRepository: IUserBoughtRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserBoughtRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> UserBoughtExists(int userId, int courseId)
    {
        var userBought = await _context.UserBoughts.FindAsync(userId, courseId);
        
        return userBought is not null;
    }

    public async Task<UserBoughtResponse> CreateAsync(UserBoughtCreateRequest userBoughtCreateRequest)
    {
        var userBought = _mapper.Map<UserBought>(userBoughtCreateRequest);

        await _context.UserBoughts.AddAsync(userBought);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserBoughtResponse>(userBought);
    }

    public async Task<bool> DeleteAsync(int userId, int courseId)
    {
        var userBought = await _context.UserBoughts.FindAsync(userId, courseId);

        if (userBought is null)
            return false;

        _context.UserBoughts.Remove(userBought);
        await _context.SaveChangesAsync();

        return true;
    }
}