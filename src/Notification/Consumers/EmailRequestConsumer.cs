using Contracts;
using Contracts.Notification;
using MassTransit;
using Notification.Dto;
using Notification.ServiceContracts;

namespace Notification.Consumers;

public class EmailRequestConsumer : IConsumer<EmailRequest>
{
    private readonly IEmailService _emailService;

    public EmailRequestConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<EmailRequest> context)
    {
        var mailDto = new MailDto()
        {
            Content = context.Message.Content,
            Subject = context.Message.Subject,
            To = context.Message.To
        };

        await _emailService.SendMailAsync(mailDto);
    }
}