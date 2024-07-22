using General.Installer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Core.Domain.RepositoryContracts;
using Profile.Infrastructure.Data;
using Profile.Infrastructure.Repositories;

namespace Profile.Infrastructure.Installers;

/// <inheritdoc />
public class InfrastructureInstaller : IInstaller
{
    /// <inheritdoc />
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(config.GetSection("ConnectionStrings:DefaultConnection").Value);
        });
        
        services.AddScoped<IProfileRepository, ProfileRepository>();
    }
}