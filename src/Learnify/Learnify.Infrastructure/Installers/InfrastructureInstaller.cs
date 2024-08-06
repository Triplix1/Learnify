using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Installer;
using Learnify.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Learnify.Infrastructure.Installers;

/// <summary>
/// InfrastructureInstaller
/// </summary>
public class InfrastructureInstaller: IInstaller
{
    /// <inheritdoc />
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IRefreshTokenRepository, RefreshTokenPsqRepository>();
        services.AddScoped<IUserRepository, UserPsqRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ICourseRatingsRepository, CourseRatingPsqRepository>();
        services.AddScoped<ICourseLessonContentRepository, CourseLessonContentRepository>();
            
        services.AddScoped<IPsqUnitOfWork, PsqUnitOfWork>();
    }
}