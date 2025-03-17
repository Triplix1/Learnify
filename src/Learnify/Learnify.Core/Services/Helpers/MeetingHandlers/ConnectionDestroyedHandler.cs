using System.Text.Json;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.MeetingConnection;
using Learnify.Core.ServiceContracts.Helpers.MeetingHandlers;

namespace Learnify.Core.Services.Helpers.MeetingHandlers;

public class ConnectionDestroyedHandler : IMeetingEventHandlers
{
    private readonly IPsqUnitOfWork _unitOfWork;

    public ConnectionDestroyedHandler(IPsqUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task HandleEvent(string json)
    {
        var connectionCreatedRequest = JsonSerializer.Deserialize<ConnectionCreatedRequest>(json);

        await _unitOfWork.MeetingConnectionRepository
            .DeleteMeetingConnectionAsync(connectionCreatedRequest.Connection.Id);
    }
}