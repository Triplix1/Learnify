using AuthIdentity.Core.Domain.RepositoryContracts;
using AuthIdentity.Infrastructure.Repositories;
using General.Installer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthIdentity.Infrastructure.Installers;

public class InfrastructureInstaller: IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}