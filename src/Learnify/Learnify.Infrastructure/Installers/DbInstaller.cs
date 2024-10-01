using Learnify.Core.Installer;
using Learnify.Infrastructure.Data;
using Learnify.Infrastructure.Data.Interfaces;
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
        });

        services.AddScoped<IMongoAppDbContext, MongoAppDbContext>();
    }
}