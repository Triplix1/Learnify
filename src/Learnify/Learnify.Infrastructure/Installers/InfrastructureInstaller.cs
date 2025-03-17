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
        services.AddScoped<ICourseRatingsRepository, CourseRatingRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();
        services.AddScoped<IParagraphRepository, ParagraphRepository>();
        services.AddScoped<IPrivateFileRepository, PrivateFileRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ISubtitlesRepository, SubtitlesRepository>();
        services.AddScoped<IUserBoughtRepository, UserBoughtRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IViewRepository, ViewRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IConnectionRepository, ConnectionRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<IUserQuizAnswerRepository, UserQuizAnswerRepository>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();
        services.AddScoped<IMeetingSessionRepository, MeetingSessionRepository>();
        services.AddScoped<IMeetingConnectionRepository, MeetingConnectionRepository>();
        
        services.AddScoped<IPsqUnitOfWork, PsqUnitOfWork>();
        services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();
    }
}