namespace Learnify.Core.ServiceContracts;

public interface IMeetingWebhookService
{
    Task HandleRequestAsync(string json);
}