namespace Learnify.Core.Domain.RepositoryContracts.UnitOfWork;

public interface IPsqUnitOfWork
{
    IUserRepository UserRepository { get; }

    IRefreshTokenRepository RefreshTokenRepository { get; }

    ICourseRatingsRepository CourseRatingsRepository { get; }

    ICourseRepository CourseRepository { get; }

    IParagraphRepository ParagraphRepository { get; }

    IPrivateFileRepository PrivateFileRepository { get; }

    IUserBoughtRepository UserBoughtRepository { get; }

    ISubtitlesRepository SubtitlesRepository { get; }

    IQuizRepository QuizRepository { get; }

    IUserQuizAnswerRepository UserQuizAnswerRepository { get; }
}