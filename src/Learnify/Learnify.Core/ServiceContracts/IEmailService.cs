using Learnify.Core.Dto;

namespace Learnify.Core.ServiceContracts;

public interface IEmailService
{
    Task SendMailAsync(MailDto messageRequest);
}