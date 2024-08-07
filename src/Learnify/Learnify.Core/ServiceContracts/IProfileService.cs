using Learnify.Core.Dto;
using Learnify.Core.Dto.Profile;

namespace Learnify.Core.ServiceContracts;

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
    Task<ApiResponse<ProfileResponse>> GetByIdAsync(int id);
    
    /// <summary>
    /// Gets all profiles
    /// </summary>
    /// <returns></returns>
    Task<ApiResponse<IEnumerable<ProfileResponse>>> GetFilteredAsync();
    
    /// <summary>
    /// Deletes profile
    /// </summary>
    /// <param name="id">Profile id</param>
    /// <returns></returns>
    Task<ApiResponse> DeleteAsync(int id);
    
    /// <summary>
    /// Updates profile
    /// </summary>
    /// <param name="profileUpdateRequest"></param>
    /// <returns></returns>
    Task<ApiResponse<ProfileResponse>> UpdateAsync(ProfileUpdateRequest profileUpdateRequest);
}