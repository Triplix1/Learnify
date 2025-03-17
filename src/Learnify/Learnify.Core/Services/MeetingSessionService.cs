using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.MeetingSession;
using Learnify.Core.ManagerContracts;
using Learnify.Core.Options;
using Learnify.Core.ServiceContracts;
using Microsoft.Extensions.Options;
using OpenTokSDK;
using Vonage.Video;
using Vonage.Video.Sessions.CreateSession;

namespace Learnify.Core.Services;

public class MeetingSessionService : IMeetingSessionService
{
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly IUserBoughtValidatorManager _userBoughtValidatorManager;
    private readonly IUserAuthorValidatorManager _userAuthorValidatorManager;
    private readonly IVideoClient _videoClient;
    private readonly VonageOptions _vonageOptions;

    public MeetingSessionService(IUserBoughtValidatorManager userBoughtValidatorManager,
        IUserAuthorValidatorManager userAuthorValidatorManager,
        IVideoClient videoClient,
        IPsqUnitOfWork psqUnitOfWork,
        IOptions<VonageOptions> vonageOptions)
    {
        _userBoughtValidatorManager = userBoughtValidatorManager;
        _userAuthorValidatorManager = userAuthorValidatorManager;
        _videoClient = videoClient;
        _psqUnitOfWork = psqUnitOfWork;
        _vonageOptions = vonageOptions.Value;
    }

    public async Task<SessionIdResponse> GetMeetingSessionForCourseAsync(int courseId, int userId,
        CancellationToken cancellationToken = default)
    {
        await _userBoughtValidatorManager.ValidateUserAccessToTheCourseAsync(userId, courseId, cancellationToken);

        var sessionResponse =
            await _psqUnitOfWork.MeetingSessionRepository.GetMeetingSessionForCourseAsync(courseId, cancellationToken);

        return new SessionIdResponse { SessionId = sessionResponse?.SessionId };
    }

    public async Task<SessionIdResponse> GetOrCreateSessionAsync(int courseId, int userId,
        CancellationToken cancellationToken = default)
    {
        await _userAuthorValidatorManager.ValidateAuthorOfCourseAsync(courseId, userId, cancellationToken);
        
        var existingSession = await _psqUnitOfWork.MeetingSessionRepository.GetMeetingSessionForCourseAsync(courseId, cancellationToken);

        if(existingSession != null)
        {
            return new SessionIdResponse { SessionId = existingSession?.SessionId };
        }
        
        // Create a session that will attempt to transmit streams directly between clients 
        var request = CreateSessionRequest.Default;
        
        // Send the request to the API
        var sessionResponse = await _videoClient.SessionClient.CreateSessionAsync(request);
        
        if (sessionResponse.IsFailure)
            throw new Exception("Error creating session");

        // var opentok = new OpenTok(_vonageOptions.ApplicationId, _vonageOptions.ApplicationKey);
        //
        // var session = await opentok.CreateSessionAsync();
        
        var meetingSessionCreateRequest = new MeetingSessionCreateRequest
        {
            SessionId = sessionResponse.GetSuccessUnsafe().SessionId,
            CourseId = courseId,
        };

        var createdMeeting =
            await _psqUnitOfWork.MeetingSessionRepository.CreateAsync(meetingSessionCreateRequest, cancellationToken);

        return new SessionIdResponse { SessionId = createdMeeting?.SessionId };
    }

    public async Task StopSessionAsync(string sessionId, int userId,
        CancellationToken cancellationToken = default)
    {
        var meetingSession = await _psqUnitOfWork.MeetingSessionRepository.GetMeetingSessionAsync(sessionId, cancellationToken);

        await _userAuthorValidatorManager.ValidateAuthorOfCourseAsync(meetingSession.CourseId, userId,
            cancellationToken);

        var success = await _psqUnitOfWork.MeetingSessionRepository.DeleteAsync(sessionId, cancellationToken);
        
        if(!success)
            throw new Exception("Error stopping session");
    }
}