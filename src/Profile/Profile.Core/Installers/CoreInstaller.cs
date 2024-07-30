using System.Net;
using System.Reflection;
using General.CommonServiceContracts;
using General.CommonServices;
using General.Installer;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Core.Consumers;
using Profile.Core.ServiceContracts;
using Profile.Core.Services;

namespace Profile.Core.Extensions;

/// <inheritdoc />
public class CoreInstaller : IInstaller
{
    private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

    /// <inheritdoc />
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(_assembly);

        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IBlobStorageGrpcService, BlobStorageGrpcService>();

        var Url = config.GetSection("GrpcSettings:BlobStorageUrl").Get<string>();
        services
            .AddGrpcClient<BlobStorage.Grpc.Protos.BlobStorage.BlobStorageClient>((services, options) =>
            {
                options.Address = new Uri(config.GetSection("GrpcSettings:BlobStorageUrl").Get<string>() ??
                                          throw new Exception("Cannot find url of blob storage service"));
            }).ConfigurePrimaryHttpMessageHandler(() => 
                new HttpClientHandler 
                { 
                    UseDefaultCredentials = true,
                    MaxConnectionsPerServer = int.MaxValue,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    AllowAutoRedirect = true,
                    MaxResponseHeadersLength = 64
                });
    }
}