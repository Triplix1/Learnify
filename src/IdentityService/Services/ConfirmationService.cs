using AutoMapper;
using IdentityService.DTOs;
using IdentityService.Models;
using IdentityService.Repository.Contracts;
using IdentityService.Services.Contracts;

namespace IdentityService.Services;

public class ConfirmationService : IConfirmationService
{
    private readonly IConfirmationRepository _confirmationRepository;
    private readonly IMapper _mapper;

    public ConfirmationService(IConfirmationRepository confirmationRepository, IMapper mapper)
    {
        _confirmationRepository = confirmationRepository;
        _mapper = mapper;
    }
    
    public async Task<ConfirmationResponse> CreateAsync(ConfirmationAddRequest confirmationAddRequest)
    {
        var confirmation = _mapper.Map<Confirmation>(confirmationAddRequest);
        var response = await _confirmationRepository.CreateAsync(confirmation);
        return _mapper.Map<ConfirmationResponse>(response);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _confirmationRepository.DeleteAsync(id);
    }

    public async Task<ConfirmationResponse> GetByEmailAsync(string email)
    {
        var confirmation = await _confirmationRepository.GetByEmailAsync(email);

        return _mapper.Map<ConfirmationResponse>(confirmation);
    }
}