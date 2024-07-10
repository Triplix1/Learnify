using Notification.Dto;

namespace Notification.ServiceContracts;

public interface IEmailService
{
    Task SendMailAsync(MailDto messageRequest);
}