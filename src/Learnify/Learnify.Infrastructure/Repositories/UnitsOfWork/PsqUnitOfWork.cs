using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;

namespace Learnify.Infrastructure.Repositories.UnitsOfWork;


public class PsqUnitOfWork: IPsqUnitOfWork
{
    public PsqUnitOfWork(IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository, ICourseRepository courseRepository,
        ICourseRatingsRepository courseRatingsRepository, IParagraphRepository paragraphRepository, IPrivateFileRepository privateFileRepository, IUserBoughtRepository userBoughtRepository, ISubtitlesRepository subtitlesRepository, IQuizRepository quizRepository)
    {
        UserRepository = userRepository;
        RefreshTokenRepository = refreshTokenRepository;
        CourseRepository = courseRepository;
        CourseRatingsRepository = courseRatingsRepository;
        ParagraphRepository = paragraphRepository;
        PrivateFileRepository = privateFileRepository;
        UserBoughtRepository = userBoughtRepository;
        SubtitlesRepository = subtitlesRepository;
        QuizRepository = quizRepository;
    }

    public IParagraphRepository ParagraphRepository { get; }

    public IPrivateFileRepository PrivateFileRepository { get; }
    
    public IUserBoughtRepository UserBoughtRepository { get; }

    public ISubtitlesRepository SubtitlesRepository { get; }

    public IQuizRepository QuizRepository { get; }

    public IUserRepository UserRepository { get; }

    public IRefreshTokenRepository RefreshTokenRepository { get; }

    public ICourseRepository CourseRepository { get; }

    public ICourseRatingsRepository CourseRatingsRepository { get; }
}