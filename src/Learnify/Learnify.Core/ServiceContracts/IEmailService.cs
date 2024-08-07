using Learnify.Core.Dto;
using Learnify.Core.Dto.Mail;

namespace Learnify.Core.ServiceContracts;

public interface IEmailService
{
    Task SendMailAsync(MailDto messageRequest);
}