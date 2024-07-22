using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace General.Installer;

/// <summary>
/// Extensions for including installers to <see cref="IServiceCollection"/>
/// </summary>
public static class InstallerExtension
{
    /// <summary>
    /// Adds installers to <see cref="IServiceCollection"/> list
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configuration"></param>
    public static void AddInstallers(this IServiceCollection app, IConfiguration configuration)
    {
        var assemblies = LoadReferencedAssemblies(AppDomain.CurrentDomain.BaseDirectory);

        // var assemblyNames = Assembly.GetCallingAssembly().GetReferencedAssemblies();
        //
        // var assemblies = assemblyNames.Select(Assembly.Load);
        
        var installers = assemblies.SelectMany(a => a.GetTypes())
                .Where(t => typeof(IInstaller).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IInstaller>();
        
        foreach (var installer in installers)
        {
            installer.InstallServices(app, configuration);
        }
    }
    
    private static IEnumerable<Assembly> LoadReferencedAssemblies(string basePath)
    {
        var loadedAssemblies = new HashSet<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
        var assemblyPaths = Directory.GetFiles(basePath, "*.dll", SearchOption.AllDirectories);

        foreach (var path in assemblyPaths)
        {
            try
            {
                var assemblyName = AssemblyName.GetAssemblyName(path);
                if (loadedAssemblies.All(a => a.GetName().FullName != assemblyName.FullName))
                {
                    var assembly = Assembly.Load(assemblyName);
                    loadedAssemblies.Add(assembly);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load assembly from path {path}: {ex.Message}");
            }
        }

        return loadedAssemblies;
    }
}