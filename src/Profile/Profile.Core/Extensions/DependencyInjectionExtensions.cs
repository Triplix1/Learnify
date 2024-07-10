using System.Reflection;
using General.CommonServiceContracts;
using General.CommonServices;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Core.Consumers;
using Profile.Core.ServiceContracts;
using Profile.Core.Services;

namespace Profile.Core.Extensions;

public static class DependencyInjectionExtensions
{
    private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly);
        
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

                cfg.Host(configuration["RabbitMq:Host"], "/", host =>
                {
                    host.Username(configuration["RabbitMq:Username"] ?? "guest");
                    host.Password(configuration["RabbitMq:Password"] ?? "guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IPhotoService, PhotoService>();
        return services;
    }
}