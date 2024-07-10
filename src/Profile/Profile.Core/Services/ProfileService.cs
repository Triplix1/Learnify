using AutoMapper;
using General.CommonServiceContracts;
using Profile.Core.Domain.Entities;
using Profile.Core.Domain.RepositoryContracts;
using Profile.Core.DTO;
using Profile.Core.ServiceContracts;

namespace Profile.Core.Services;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;
    private readonly IPhotoService _photoService;
    private readonly IMapper _mapper;

    public ProfileService(IProfileRepository profileRepository, IMapper mapper, IPhotoService photoService)
    {
        _profileRepository = profileRepository;
        _mapper = mapper;
        _photoService = photoService;
    }

    public async Task<ProfileResponse> GetByIdAsync(string id)
    {
        var profile = await _profileRepository.GetByIdAsync(id);

        return _mapper.Map<ProfileResponse>(profile);
    }

    public async Task<IEnumerable<ProfileResponse>> GetAllProfilesAsync()
    {
        var profiles = await _profileRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<ProfileResponse>>(profiles);
    }

    public async Task DeleteAsync(string id)
    {
        var profile = await _profileRepository.GetByIdAsync(id);
        
        if(profile is null)
            throw new KeyNotFoundException("Cannot find user with such id");
        
        if (!await _profileRepository.DeleteAsync(profile))
        {
            throw new KeyNotFoundException("Cannot find user with such id");
        }

        if(profile.PhotoPublicId is not null)
            await _photoService.DeletePhotoAsync(profile.PhotoPublicId);
    }

    public async Task<ProfileResponse> UpdateAsync(ProfileUpdateRequest profileUpdateRequest)
    {
        var origin = await _profileRepository.GetByIdAsync(profileUpdateRequest.Id);

        if (origin is null)
            throw new KeyNotFoundException("Cannot find user with such id");
        
        var profile = _mapper.Map<User>(profileUpdateRequest);

        if (profileUpdateRequest.File is not null)
        {
            if (origin.PhotoPublicId is not null)
                await _photoService.DeletePhotoAsync(origin.PhotoPublicId);

            var photoResult = await _photoService.AddPhotoAsync(profileUpdateRequest.File, 500, 500);

            profile.PhotoUrl = photoResult.Url.AbsoluteUri;

            profile.PhotoPublicId = photoResult.PublicId;
        }
        else
        {
            profile.PhotoUrl = origin.PhotoUrl;
            profile.PhotoPublicId = origin.PhotoPublicId;
        }

        return _mapper.Map<ProfileResponse>(await _profileRepository.UpdateAsync(profile));
    }
}