using Learnify.Core.Dto;
using Learnify.Core.Dto.Profile;

namespace Learnify.Core.ServiceContracts;

/// <summary>
/// Service for profile management
/// </summary>
public interface IProfileService
{
    Task<ProfileResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<ProfileResponse> UpdateAsync(ProfileUpdateRequest profileUpdateRequest,
        CancellationToken cancellationToken = default);
}