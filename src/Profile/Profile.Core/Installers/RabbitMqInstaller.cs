using General.Installer;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Core.Consumers;

namespace Profile.Core.Extensions;

public class RabbitMqInstaller: IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumersFromNamespaceContaining<UserCreatedConsumer>();

            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("profile", false));

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseMessageRetry(r =>
                {
                    r.Handle<RabbitMqConnectionException>();
                    r.Interval(5, TimeSpan.FromSeconds(10));
                });

                cfg.Host(config["RabbitMq:Host"], "/", host =>
                {
                    host.Username(config["RabbitMq:Username"] ?? "guest");
                    host.Password(config["RabbitMq:Password"] ?? "guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}