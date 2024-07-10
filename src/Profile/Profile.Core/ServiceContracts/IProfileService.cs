using Profile.Core.DTO;

namespace Profile.Core.ServiceContracts;

public interface IProfileService
{
    Task<ProfileResponse> GetByIdAsync(string id);
    Task<IEnumerable<ProfileResponse>> GetAllProfilesAsync();
    Task DeleteAsync(string id);
    Task<ProfileResponse> UpdateAsync(ProfileUpdateRequest profileUpdateRequest);
}