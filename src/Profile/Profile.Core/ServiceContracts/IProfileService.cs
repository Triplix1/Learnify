using General.Dto;
using Profile.Core.DTO;

namespace Profile.Core.ServiceContracts;

/// <summary>
/// Service for profile management
/// </summary>
public interface IProfileService
{
    /// <summary>
    /// Gets profile by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ApiResponse<ProfileResponse>> GetByIdAsync(string id);
    
    /// <summary>
    /// Gets all profiles
    /// </summary>
    /// <returns></returns>
    Task<ApiResponse<IEnumerable<ProfileResponse>>> GetAllProfilesAsync();
    
    /// <summary>
    /// Deletes profile
    /// </summary>
    /// <param name="id">Profile id</param>
    /// <returns></returns>
    Task<ApiResponse> DeleteAsync(string id);
    
    /// <summary>
    /// Updates profile
    /// </summary>
    /// <param name="profileUpdateRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<ProfileResponse>> UpdateAsync(ProfileUpdateRequest profileUpdateRequest);
}