using General.Installer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthIdentity.Infrastructure.Installers;

public class DbInstaller: IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseNpgsql(config["ConnectionStrings:DefaultConnection"]);
            var s = config["ConnectionStrings:DefaultConnection"];
        });
    }
}