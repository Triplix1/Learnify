using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Infrastructure.Data;

namespace Learnify.Infrastructure.Repositories.UnitsOfWork;

/// <inheritdoc />
public class PsqUnitOfWork: IPsqUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of <see cref="PsqUnitOfWork"/>
    /// </summary>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    /// <param name="userRepository"><see cref="IUserRepository"/></param>
    /// <param name="refreshTokenRepository"><see cref="IRefreshTokenRepository"/></param>
    public PsqUnitOfWork(ApplicationDbContext context, IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository, ICourseRepository courseRepository,
        ICourseRatingsRepository courseRatingsRepository, IParagraphRepository paragraphRepository)
    {
        _context = context;
        UserRepository = userRepository;
        RefreshTokenRepository = refreshTokenRepository;
        CourseRepository = courseRepository;
        CourseRatingsRepository = courseRatingsRepository;
        ParagraphRepository = paragraphRepository;
    }

    private readonly ApplicationDbContext _context;

    /// <inheritdoc />
    public IParagraphRepository ParagraphRepository { get; }

    /// <inheritdoc />
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public IUserRepository UserRepository { get; }

    /// <inheritdoc />
    public IRefreshTokenRepository RefreshTokenRepository { get; }

    /// <inheritdoc />
    public ICourseRepository CourseRepository { get; }

    /// <inheritdoc />
    public ICourseRatingsRepository CourseRatingsRepository { get; }
}