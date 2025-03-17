using System.Text.Json;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.MeetingConnection;
using Learnify.Core.Dto.MeetingSession;
using Learnify.Core.ManagerContracts;
using Learnify.Core.Options;
using Learnify.Core.ServiceContracts.Helpers.MeetingHandlers;
using Microsoft.Extensions.Options;
using Vonage.Video;
using Vonage.Video.Moderation.DisconnectConnection;

namespace Learnify.Core.Services.Helpers.MeetingHandlers;

public class ConnectionCreatedHandler : IMeetingEventHandlers
{
    private readonly IPsqUnitOfWork _unitOfWork;
    private readonly IUserAuthorValidatorManager _userAuthorValidatorManager;
    private readonly IVideoClient _videoClient;
    private readonly VonageOptions _vonageOptions;

    public ConnectionCreatedHandler(IPsqUnitOfWork unitOfWork,
        IUserAuthorValidatorManager userAuthorValidatorManager,
        IVideoClient videoClient,
        IOptions<VonageOptions> vonageOptions)
    {
        _unitOfWork = unitOfWork;
        _userAuthorValidatorManager = userAuthorValidatorManager;
        _videoClient = videoClient;
        _vonageOptions = vonageOptions.Value;
    }

    public async Task HandleEvent(string json)
    {
        var connectionCreatedRequest = JsonSerializer.Deserialize<ConnectionCreatedRequest>(json);

        try
        {
            if (string.IsNullOrWhiteSpace(connectionCreatedRequest.Connection.Data))
            {
                await DisconnectAsync(connectionCreatedRequest.SessionId, connectionCreatedRequest.Connection.Id);
                throw new ApplicationException("User Id not provided");
            }

            var userId = int.Parse(connectionCreatedRequest.Connection.Data);

            var meetingSession =
                await _unitOfWork.MeetingSessionRepository.GetMeetingSessionAsync(connectionCreatedRequest.SessionId);

            if (meetingSession == null)
            {
                await DisconnectAsync(connectionCreatedRequest.SessionId, connectionCreatedRequest.Connection.Id);
                throw new KeyNotFoundException("Cannot find meeting session");
            }

            var isAuthor = await IsCourseAuthor(meetingSession, userId);

            var meetingConnectionCreateRequest = new MeetingConnectionCreateRequest()
            {
                SessionId = connectionCreatedRequest.SessionId,
                ConnectionId = connectionCreatedRequest.Connection.Id,
                UserId = userId,
                IsAuthor = isAuthor,
            };

            await _unitOfWork.MeetingConnectionRepository.CreateMeetingConnectionAsync(meetingConnectionCreateRequest);
        }
        catch (Exception)
        {
            var disconnectRequest = DisconnectConnectionRequest.Build()
                .WithApplicationId(Guid.Parse((ReadOnlySpan<char>)_vonageOptions.ApplicationId))
                .WithSessionId(connectionCreatedRequest.SessionId)
                .WithConnectionId(connectionCreatedRequest.Connection.Id)
                .Create();

            await _videoClient.ModerationClient.DisconnectConnectionAsync(disconnectRequest);
        }
    }

    private async Task<bool> IsCourseAuthor(MeetingSessionResponse meetingSession, int userId)
    {
        try
        {
            await _userAuthorValidatorManager.ValidateAuthorOfCourseAsync(meetingSession.CourseId, userId);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    private async Task DisconnectAsync(string sessionId, string connectionId)
    {
        var request = DisconnectConnectionRequest.Build()
            .WithApplicationId(Guid.Parse((ReadOnlySpan<char>)_vonageOptions.ApplicationId))
            .WithSessionId(sessionId)
            .WithConnectionId(connectionId)
            .Create();

        await _videoClient.ModerationClient.DisconnectConnectionAsync(request);
    }
}