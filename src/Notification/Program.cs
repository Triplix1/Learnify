using MassTransit;
using Notification.Config;
using Notification.Consumers;
using Notification.ServiceContracts;
using Notification.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.Configure<MailConfig>(builder.Configuration.GetSection("MailConfig"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<EmailRequestConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("notification", false));

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.UseRetry(r =>
        {
            r.Handle<RabbitMqConnectionException>();
            r.Interval(5, TimeSpan.FromSeconds(10));
        });

        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();