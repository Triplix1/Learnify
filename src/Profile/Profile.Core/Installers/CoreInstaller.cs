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

public class CoreInstaller : IInstaller
{
    private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(_assembly);

        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IPhotoService, PhotoService>();
    }
}