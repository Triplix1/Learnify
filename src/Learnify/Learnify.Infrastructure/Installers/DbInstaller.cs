using Learnify.Core.Installer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Learnify.Infrastructure.Installers;

/// <summary>
/// DbInstaller
/// </summary>
public class DbInstaller: IInstaller
{
    /// <inheritdoc />
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(config["ConnectionStrings:DefaultConnection"]);
            var s = config["ConnectionStrings:DefaultConnection"];
        });
    }
}