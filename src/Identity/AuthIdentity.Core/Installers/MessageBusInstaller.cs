using AuthIdentity.Core.Consumers;
using General.Installer;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthIdentity.Core.Installers;

public class MessageBusInstaller: IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumersFromNamespaceContaining<UserUpdatedConsumer>();
            
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("identity", false));

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseMessageRetry(r =>
                {
                    r.Handle<RabbitMqConnectionException>();
                    r.Interval(5, TimeSpan.FromSeconds(10));
                });

                cfg.Host(config["RabbitMq:Host"], "/", host =>
                {
                    host.Username(config.GetValue("RabbitMq:Username", "guest"));
                    host.Password(config.GetValue("RabbitMq:Password", "guest"));
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}