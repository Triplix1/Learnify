using System.Reflection;
using Azure.Storage.Blobs;
using BlobStorage.Core.Options;
using BlobStorage.Core.Services.Contracts;
using General.Installer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlobStorage.Core.Installers;

public class CoreInstaller: IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddSingleton(x => new BlobServiceClient(config.GetConnectionString("BlobStorageSettings")));
        
        services.AddScoped<IBlobStorage, Services.BlobStorage>();
    }
}