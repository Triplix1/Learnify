using Learnify.Core.Dto.MeetingConnection;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IMeetingConnectionRepository
{
    Task<MeetingConnectionResponse> CreateMeetingConnectionAsync(MeetingConnectionCreateRequest meetingConnectionCreateRequest);
    Task<bool> DeleteMeetingConnectionAsync(string connectionId);
}