using AutoMapper;
using IdentityService.DTOs.TemporaryUser;
using IdentityService.Models;
using IdentityService.Repository.Contracts;
using IdentityService.Services.Contracts;

namespace IdentityService.Services;

public class TemporaryUserService : ITemporaryUserService
{
    private readonly ITemporaryUserRepository _temporaryUserRepository;
    private readonly IMapper _mapper;

    public TemporaryUserService(ITemporaryUserRepository temporaryUserRepository, IMapper mapper)
    {
        _temporaryUserRepository = temporaryUserRepository;
        _mapper = mapper;
    }

    public async Task<TemporaryUserResponse> CreateAsync(TemporaryUserAddRequest temporaryUserAddRequest)
    {
        var temporaryUser = _mapper.Map<TemporaryUser>(temporaryUserAddRequest);

        await _temporaryUserRepository.CreateAsync(temporaryUser);
        
        return _mapper.Map<TemporaryUserResponse>(temporaryUser);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _temporaryUserRepository.DeleteAsync(id);
    }

    public async Task<TemporaryUserResponse> GetByIdAsync(Guid id)
    {
        var result = await _temporaryUserRepository.GetByIdAsync(id);

        return _mapper.Map<TemporaryUserResponse>(result);
        
    }
}