using Learnify.Core.Dto.MeetingToken;

namespace Learnify.Core.ServiceContracts;

public interface IMeetingTokenService
{
    Task<MeetingTokenResponse> GenerateMeetingTokenAsync(string sessionId, int userId, CancellationToken cancellationToken = default);
}