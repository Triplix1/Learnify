using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Infrastructure.Data;

namespace Learnify.Infrastructure.Repositories.UnitsOfWork;

/// <inheritdoc />
public class PsqUnitOfWork: IPsqUnitOfWork
{
    public PsqUnitOfWork(ApplicationDbContext context, IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository, ICourseRepository courseRepository,
        ICourseRatingsRepository courseRatingsRepository, IParagraphRepository paragraphRepository, IPrivateFileRepository privateFileRepository, IUserBoughtRepository userBoughtRepository, ISubtitlesRepository subtitlesRepository)
    {
        _context = context;
        UserRepository = userRepository;
        RefreshTokenRepository = refreshTokenRepository;
        CourseRepository = courseRepository;
        CourseRatingsRepository = courseRatingsRepository;
        ParagraphRepository = paragraphRepository;
        PrivateFileRepository = privateFileRepository;
        UserBoughtRepository = userBoughtRepository;
        SubtitlesRepository = subtitlesRepository;
    }

    private readonly ApplicationDbContext _context;

    /// <inheritdoc />
    public IParagraphRepository ParagraphRepository { get; }

    /// <inheritdoc />
    public IPrivateFileRepository PrivateFileRepository { get; }
    
    /// <inheritdoc />
    public IUserBoughtRepository UserBoughtRepository { get; }

    /// <inheritdoc />
    public ISubtitlesRepository SubtitlesRepository { get; }

    /// <inheritdoc />
    public IUserRepository UserRepository { get; }

    /// <inheritdoc />
    public IRefreshTokenRepository RefreshTokenRepository { get; }

    /// <inheritdoc />
    public ICourseRepository CourseRepository { get; }

    /// <inheritdoc />
    public ICourseRatingsRepository CourseRatingsRepository { get; }
}