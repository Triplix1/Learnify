using Learnify.Core.Dto.MeetingSession;

namespace Learnify.Core.ServiceContracts;

public interface IMeetingSessionService
{
    Task<SessionIdResponse> GetMeetingSessionForCourseAsync(int courseId, int userId,
        CancellationToken cancellationToken = default);

    Task<SessionIdResponse> GetOrCreateSessionAsync(int courseId, int userId,
        CancellationToken cancellationToken = default);

    Task StopSessionAsync(string sessionId, int userId, CancellationToken cancellationToken = default);
}