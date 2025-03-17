using System.Text.Json;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.MeetingSession;
using Learnify.Core.ServiceContracts.Helpers.MeetingHandlers;

namespace Learnify.Core.Services.Helpers.MeetingHandlers;

public class SessionDestroyedHandler: IMeetingEventHandlers
{
    private readonly IPsqUnitOfWork _unitOfWork;

    public SessionDestroyedHandler(IPsqUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task HandleEvent(string json)
    {
        var connectionCreatedRequest = JsonSerializer.Deserialize<SessionDestroyedRequest>(json);

        await _unitOfWork.MeetingSessionRepository.DeleteAsync(connectionCreatedRequest.SessionId);
    }
}