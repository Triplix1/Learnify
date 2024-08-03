using Learnify.Core.Dto;
using Learnify.Core.Options;
using Learnify.Core.ServiceContracts;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Learnify.Core.Services;

public class EmailService : IEmailService
{
    private readonly MailOptions _mailOptions;

    public EmailService(IOptions<MailOptions> mailConfig)
    {
        _mailOptions = mailConfig.Value;
    }
    
    public async Task SendMailAsync(MailDto messageRequest)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailOptions.Mail);
        email.To.AddRange(messageRequest.To.Select(email => MailboxAddress.Parse(email)));
        email.Subject = messageRequest.Subject;

        var builder = new BodyBuilder
        {
            HtmlBody = messageRequest.Content
        };
        
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_mailOptions.Host, _mailOptions.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_mailOptions.Mail, _mailOptions.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}