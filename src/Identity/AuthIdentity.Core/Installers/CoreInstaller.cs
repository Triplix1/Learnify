using System.Reflection;
using AuthIdentity.Core.ManagerContracts;
using AuthIdentity.Core.Managers;
using AuthIdentity.Core.Options;
using AuthIdentity.Core.ServiceContracts;
using AuthIdentity.Core.Services;
using FluentValidation;
using General.Installer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace AuthIdentity.Core.Installers;

public class CoreInstaller: IInstaller
{        
    public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddEndpointsApiExplorer();
        services.AddControllers();

        services.AddSwaggerGen();
        
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IGoogleAuthManager, GoogleAuthManager>();
        services.AddScoped<IIdentityService, IdentityService>();

        services.Configure<GoogleAuthOptions>(config.GetSection("GoogleAuthSettings"));
        services.Configure<JwtOptions>(config.GetSection("JwtSettings"));

        services.AddValidatorsFromAssembly(Assembly);
        services.AddFluentValidationAutoValidation();
        services.AddHttpClient();

        services.AddAutoMapper(Assembly);
    }
}