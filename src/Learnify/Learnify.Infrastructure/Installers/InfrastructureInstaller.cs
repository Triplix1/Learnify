using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Installer;
using Learnify.Infrastructure.Repositories;
using Learnify.Infrastructure.Repositories.UnitsOfWork;
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
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ICourseRatingsRepository, CourseRatingRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();
        services.AddScoped<IParagraphRepository, ParagraphRepository>();
        
        services.AddScoped<IPsqUnitOfWork, PsqUnitOfWork>();
        services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();
    }
}