using Learnify.Core.Domain.RepositoryContracts;

namespace Learnify.Infrastructure.Repositories;

/// <inheritdoc />
public class UnitOfWork: IUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of <see cref="UnitOfWork"/>
    /// </summary>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    /// <param name="userRepository"><see cref="IUserRepository"/></param>
    /// <param name="refreshTokenRepository"><see cref="IRefreshTokenRepository"/></param>
    public UnitOfWork(ApplicationDbContext context, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
    {
        _context = context;
        UserRepository = userRepository;
        RefreshTokenRepository = refreshTokenRepository;
    }

    private readonly ApplicationDbContext _context;
    
    /// <inheritdoc />
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public IUserRepository UserRepository { get; }

    /// <inheritdoc />
    public IRefreshTokenRepository RefreshTokenRepository { get; }
}