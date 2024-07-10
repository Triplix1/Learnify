using BlobStorage.Core.Consumers;
using General.Installer;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlobStorage.Core.Installers;

public class RabbitMqInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumersFromNamespaceContaining<AddBlobConsumer>();

            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("blob", false));

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseRetry(r =>
                {
                    r.Handle<RabbitMqConnectionException>();
                    r.Interval(5, TimeSpan.FromSeconds(10));
                });

                cfg.Host(config["RabbitMq:Host"], "/", host =>
                {
                    host.Username(config.GetSection("RabbitMq:Username").Value ?? "guest");
                    host.Password(config.GetSection("RabbitMq:Password").Value ?? "guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}