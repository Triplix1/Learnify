using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.MeetingToken;
using Learnify.Core.ManagerContracts;
using Learnify.Core.Options;
using Learnify.Core.ServiceContracts;
using Microsoft.Extensions.Options;
using OpenTokSDK;
using Vonage.Request;
using Vonage.Video.Authentication;
using Role = Vonage.Video.Authentication.Role;

namespace Learnify.Core.Services;

public class MeetingTokenService : IMeetingTokenService
{
    private readonly IVideoTokenGenerator _videoTokenGenerator;
    private readonly IUserBoughtValidatorManager _userBoughtValidatorManager;
    private readonly IUserAuthorValidatorManager _userAuthorValidatorManager;
    private readonly IPsqUnitOfWork _psqUnitOfWork;
    private readonly Credentials _credentials;

    public MeetingTokenService(IVideoTokenGenerator videoTokenGenerator,
        IUserBoughtValidatorManager userBoughtValidatorManager,
        IPsqUnitOfWork psqUnitOfWork,
        IOptions<VonageOptions> vonageOptions,
        IUserAuthorValidatorManager userAuthorValidatorManager)
    {
        _videoTokenGenerator = videoTokenGenerator;
        _userBoughtValidatorManager = userBoughtValidatorManager;
        _psqUnitOfWork = psqUnitOfWork;
        _userAuthorValidatorManager = userAuthorValidatorManager;

        _credentials =
            Credentials.FromAppIdAndPrivateKey(vonageOptions.Value.ApplicationId, vonageOptions.Value.ApplicationKey);
    }

    public async Task<MeetingTokenResponse> GenerateMeetingTokenAsync(string sessionId, int userId,
        CancellationToken cancellationToken = default)
    {
        var meetingSession =
            await _psqUnitOfWork.MeetingSessionRepository.GetMeetingSessionAsync(sessionId, cancellationToken);

        if (meetingSession == null)
            throw new KeyNotFoundException("Meeting session not found");

        await _userBoughtValidatorManager.ValidateUserAccessToTheCourseAsync(userId, meetingSession.CourseId,
            cancellationToken);

        bool isUserAuthorOfTheCourse;

        try
        {
            await _userAuthorValidatorManager.ValidateAuthorOfCourseAsync(meetingSession.CourseId, userId,
                cancellationToken);
            isUserAuthorOfTheCourse = true;
        }
        catch
        {
            isUserAuthorOfTheCourse = false;
        }

        // var role = OpenTokSDK.Role.PUBLISHER;
        //
        // if (isUserAuthorOfTheCourse)
        // {
        //     role = OpenTokSDK.Role.MODERATOR;
        // }
        //
        // var opentok = new OpenTok(_credentials.ApplicationId, _credentials.ApplicationKey);
        //
        // string connectionMetadata = $"UserId={userId}";
        // var token = opentok.GenerateToken(sessionId, role, 0, connectionMetadata);
        
        var role = Role.Publisher;
        
        if (isUserAuthorOfTheCourse)
        {
            role = Role.Moderator;
        }

        var claims = new Dictionary<string, object>()
        {
            { "connection_data", userId.ToString() }
        };

        var tokenAdditionClaims = TokenAdditionalClaims.Parse(sessionId, role: role, claims: claims);
        
        var tokenResult = _videoTokenGenerator.GenerateToken(_credentials, tokenAdditionClaims);
        
        if (tokenResult.IsFailure)
            throw new KeyNotFoundException("Token generation failed");

        return new MeetingTokenResponse
        {
            Token = tokenResult.GetSuccessUnsafe().Token
        };
    }
}