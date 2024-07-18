using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace General.Installer;

/// <summary>
/// Installs dependencies
/// </summary>
public interface IInstaller
{
    /// <summary>
    /// Installs dependencies
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="config"><see cref="IConfiguration"/></param>
    void InstallServices(IServiceCollection services, IConfiguration config);
}