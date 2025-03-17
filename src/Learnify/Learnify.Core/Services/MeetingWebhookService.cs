using System.Text.Json;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.ManagerContracts;
using Learnify.Core.Options;
using Learnify.Core.ServiceContracts;
using Learnify.Core.ServiceContracts.Helpers.MeetingHandlers;
using Learnify.Core.Services.Helpers.MeetingHandlers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vonage.Video;

namespace Learnify.Core.Services;

public class MeetingWebhookService : IMeetingWebhookService
{
    private readonly IPsqUnitOfWork _unitOfWork;
    private readonly IUserAuthorValidatorManager _userAuthorValidatorManager;
    private readonly IVideoClient _videoClient;
    private readonly IOptions<VonageOptions> _vonageOptions;
    private readonly ILogger<MeetingWebhookService> _logger;

    public MeetingWebhookService(IPsqUnitOfWork unitOfWork,
        IUserAuthorValidatorManager userAuthorValidatorManager,
        IVideoClient videoClient,
        IOptions<VonageOptions> vonageOptions,
        ILogger<MeetingWebhookService> logger)
    {
        _unitOfWork = unitOfWork;
        _userAuthorValidatorManager = userAuthorValidatorManager;
        _videoClient = videoClient;
        _vonageOptions = vonageOptions;
        _logger = logger;
    }

    public async Task HandleRequestAsync(string json)
    {
        var eventName = string.Empty;
        using (var doc = JsonDocument.Parse(json))
        {
            eventName = doc.RootElement.GetProperty("event").GetString();
        }

        IMeetingEventHandlers meetingEventHandler = null;

        switch (eventName)
        {
            case "connectionCreated":
                meetingEventHandler = new ConnectionCreatedHandler(_unitOfWork, _userAuthorValidatorManager,
                    _videoClient, _vonageOptions);
                break;
            case "connectionDestroyed":
                meetingEventHandler = new ConnectionDestroyedHandler(_unitOfWork);
                break;
            case "sessionDestroyed":
                meetingEventHandler = new SessionDestroyedHandler(_unitOfWork);
                break;
        }

        if (meetingEventHandler != null)
        {
            _logger.LogInformation($"Handing event from Vonage: {eventName}");
            await meetingEventHandler.HandleEvent(json);
        }
    }
}