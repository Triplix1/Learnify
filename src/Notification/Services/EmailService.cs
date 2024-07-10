using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Notification.Config;
using Notification.Dto;
using Notification.ServiceContracts;

namespace Notification.Services;

public class EmailService : IEmailService
{
    private readonly MailConfig _mailConfig;

    public EmailService(IOptions<MailConfig> mailConfig)
    {
        _mailConfig = mailConfig.Value;
    }
    
    public async Task SendMailAsync(MailDto messageRequest)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailConfig.Mail);
        email.To.AddRange(messageRequest.To.Select(email => MailboxAddress.Parse(email)));
        email.Subject = messageRequest.Subject;

        var builder = new BodyBuilder
        {
            HtmlBody = messageRequest.Content
        };
        
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_mailConfig.Host, _mailConfig.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_mailConfig.Mail, _mailConfig.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}