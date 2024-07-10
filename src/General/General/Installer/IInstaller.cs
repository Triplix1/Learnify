using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace General.Installer;

public interface IInstaller
{
    void InstallServices(IServiceCollection services, IConfiguration config);
}