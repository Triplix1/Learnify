using Learnify.Core.Dto.MeetingSession;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IMeetingSessionRepository
{
    Task<MeetingSessionResponse> GetMeetingSessionForCourseAsync(int courseId,
        CancellationToken cancellationToken = default);

    Task<MeetingSessionResponse>
        GetMeetingSessionAsync(string sessionId, CancellationToken cancellationToken = default);

    Task<MeetingSessionResponse> CreateAsync(MeetingSessionCreateRequest meetingSessionCreateRequest,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(string sessionId, CancellationToken cancellationToken = default);
}