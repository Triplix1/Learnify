namespace Learnify.Core.ServiceContracts.Helpers.MeetingHandlers;

public interface IMeetingEventHandlers
{
    Task HandleEvent(string json);
}